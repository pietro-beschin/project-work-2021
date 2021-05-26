// ISTANZIO LE VARIABILI GLOBALI
const baseURL = 'http://54.85.250.76:3000/api/';
let datiCommesse;
let nomeArticolo;
let graphData;

// METODI RICHIAMATI AL LOAD
$(document).ready(function () {
    $('#div-nessun-risultato').hide();
    initTable();
    fetchAllData();

    //#region TIMER DI AGGIORNAMENTO DATI

    // timer di aggiornamento della sezione stato macchina
    setInterval(statusPolling, 1000);
    function statusPolling() {
        $.ajax({
            type: 'GET',
            url: `${baseURL}lastCommessaStatus`,
        }).then(commessa => {
            $('#quadrante-lavorazione').html(`${commessa.history.articolo}`);
            $('#quadrante-stato').html(`${commessa.status.stato}`);
            $('#quadrante-progresso').html(`${commessa.history.quantita_prodotta}/${commessa.history.quantita_prevista}`);
            $('#quadrante-progresso-percentuale').html(`${((commessa.history.quantita_prodotta * 100) / commessa.history.quantita_prevista).toFixed(1)}%`);
            $('#quadrante-allarmi').html(`${commessa.status.allarme}`);
        });
    };

    // timer di aggiornamento della sezione storico commesse
    setInterval(historyPolling, 2000);
    function historyPolling() {
        fetchNewCommesse();
    };
    //#endregion

    //#region METODI RELATIVI AGLI EVENTI CLIENT
    $('#btnCerca').click((event) => {
        event.preventDefault();
        let table = $('#dt-commesse').DataTable();
        table.clear().draw();
        datiCommesse = [];
        fetchNewCommesse()
    });

    $('#btnClearFilters').click(() => {
        $('#nome-articolo').val('');
        $('#dtp-inizio').val('');
        $('#dtp-fine').val('');
        let table = $('#dt-commesse').DataTable();
        table.clear().draw();
        datiCommesse = [];
        fetchNewCommesse()
    });

    $('input[name="switch-completato"]').change(() => {
        let table = $('#dt-commesse').DataTable();
        table.clear().draw();
        datiCommesse = [];
        fetchNewCommesse()
        //toggle404(datiCommesse);
    });
    //#endregion
});

const toggle404 = (dati) => {
    if(!dati.length) {
        $('#dt-commesse_wrapper').hide()
        $('#div-nessun-risultato').show()
    } else {
        $('#div-nessun-risultato').hide()
        $('#dt-commesse_wrapper').show()
    };
};

//#region METODI PER LE CHIAMATE ALLE API

// chiamata API effettuata al caricamento della pagina
const fetchAllData = () => {
    $.ajax({
        type: 'GET',
        url: `${baseURL}historyStatus`,
    }).then(result => {
        datiCommesse = result.history;

        // popolo lo storico commesse
        $('#accordion-commesse').empty();
        datiCommesse.forEach(commessa => {
            if ($('#switch-completato').is(":checked") === true) {
                if (commessa.stato === 'fallita') {
                    addCommessaToList(commessa)
                }
            } else {
                addCommessaToList(commessa)
            }
            updateTableRow(commessa);
        });

        // popolo i dati sulla sezione stato macchina
        let pos = datiCommesse.length - 1;
        $('#quadrante-lavorazione').html(`${datiCommesse[datiCommesse.length - 1].articolo}`);
        $('#quadrante-stato').html(`${result.status.stato}`);
        $('#quadrante-progresso').html(`${datiCommesse[pos].quantita_prodotta}/${datiCommesse[pos].quantita_prevista}`);
        $('#quadrante-progresso-percentuale').html(`${((datiCommesse[pos].quantita_prodotta * 100) / datiCommesse[pos].quantita_prevista).toFixed(1)}%`);
        $('#quadrante-allarmi').html(`${result.status.allarme}`);

        // svuoto la tabella per riempirla dei nuovi dati
        let table = $('#dt-commesse').DataTable();
        table.rows().invalidate().draw(true);

        // renderizzo il grafico
        graphData = formatGraphData(datiCommesse);
        renderGraph();
    }).catch(() => {
        //gestione della pagina in caso di mancanza di connessione al server
        datiCommesse = [];
        toggle404(datiCommesse);
        $('#quadrante-lavorazione').html('---');
        $('#quadrante-stato').html('---');
        $('#quadrante-progresso').html('---');
        $('#quadrante-progresso-percentuale').html('---');
        $('#quadrante-allarmi').html('---');
    });
};

// chiamata API per la ricerca delle commesse e aggiornamento dell'elenco commesse
const fetchNewCommesse = () => {
    nomeArticolo = document.getElementById("nome-articolo").value;
    let startDate = $("#dtp-inizio").val();
    let endDate = $("#dtp-fine").val();

    if (event && event.preventDefault) {
        event.preventDefault();
    };

    $.ajax({
        type: 'GET',
        url: `${baseURL}history?articolo=${nomeArticolo}&from=${startDate}&to=${endDate}`,
    }).then(result => {
        // confronto le commesse già presenti con la nuova chiamata e aggiungo solo i nuovi elementi all'elenco
        let diff = _.differenceBy(result, datiCommesse, '_id');

        diff.forEach(commessa => {
            if ($('#switch-completato').is(":checked") === true) {
                if (commessa.stato === 'fallita') {
                    addCommessaToList(commessa)
                }
            } else {
                addCommessaToList(commessa)
            }
        });
        result.forEach(commessa => {
            updateTableRow(commessa);
        });
        document.getElementById("nome-articolo").value = nomeArticolo;
        
        datiCommesse = result;

        toggle404(result);
    }).catch(() => {
        datiCommesse = [];
        toggle404(datiCommesse);
        $('#quadrante-lavorazione').html('---');
        $('#quadrante-stato').html('---');
        $('#quadrante-progresso').html('---');
        $('#quadrante-progresso-percentuale').html('---');
        $('#quadrante-allarmi').html('---');
    });
};
//#endregion

//#region METODI PER LO STORICO COMMESSE

// istanzio l'oggetto datatable per inserire le commesse nell'elenco
const initTable = (function () {
    let table = $('#dt-commesse');

    table.DataTable().destroy()
    table.DataTable({
        "columnDefs":[{
            type: 'item',
            targets: 0
        }],
        "order": [[0, 'desc']],
        "serverSide": false,
        "iDisplayLength": 10,
        "paging": true,
        "lengthChange": true,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        lengthMenu: [
            [5, 10, 25, -1],
            ['5', '10', '25', 'Tutti']
        ],
        "language": {
            "lengthMenu": "Mostra _MENU_ risultati per pagina",
            "info": "Pagina _PAGE_ di _PAGES_",
            "infoEmpty": "",
            "infoFiltered": " - Filtrato da _MAX_ risultati",
            "emptyTable": " ",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": ">",
                "previous": "<"
            }
        }
    });

    // algoritmi di ordinamento dei risultati nello storico commesse
    $.fn.dataTableExt.oSort["item-desc"] = function(a, b) {
        a = $(a).attr('item-date');
        b = $(b).attr('item-date');
        a = new Date(a).getTime();
        b = new Date(b).getTime();
        if (a > b) {
            return -1;
        }
        if (a < b) {
            return 1;
        }
        return 0;
    };

    $.fn.dataTableExt.oSort["item-asc"] = function(a, b) {
        a = $(a).attr('item-date');
        b = $(b).attr('item-date');
        a = new Date(a).getTime();
        b = new Date(b).getTime();
        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;
    }
});

// codice html generato dinamicamente per le singole entry nello storico commesse
const addCommessaToList = (commessa) => {
    const template = $(`
    <tr class="table-item">
        <td>
            <div id="table_${commessa._id}" class="accordion md-accordion" role="tablist" item-date="${commessa.data_esecuzione}">
                <div class="card lighter-back-dark-gradient border-lighter">
                    <div class="card-header" role="tab" id="heading_${commessa._id}">
                        <a data-toggle="collapse" data-parent="#accordion-commesse" href="#collapse_${commessa._id}" aria-expanded="false" aria-controls="collapse_${commessa._id}">
                            <div class="row">
                                <div class="stato-container col-md-auto mt-2"></div>
                                <div class="col-lg-7 h4 text-light mt-2">${commessa.codice_commessa}</div>
                                <div class="col-md-auto text-light mt-2 item-data_di_esecuzione">${dateFormat(commessa.data_aggiornamento)}</div>       
                            </div>
                        </a>
                    </div>
                    <div id="collapse_${commessa._id}" class="collapse med-back" role="tabpanel" aria-labelledby="heading_${commessa._id}" data-parent="accordion-commesse">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>codice commessa</strong></div>
                                <div class="col-md-auto">${commessa.codice_commessa}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div><div class="col-sm-3"><strong>data ultima esecuzione</strong></div>
                                <div class="item-data-esecuzione col-md-auto item-data_di_esecuzione">${dateFormat(commessa.data_aggiornamento)}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div><div class="col-sm-3"><strong>data inizio commessa</strong></div>
                                <div class="item-data-esecuzione col-md-auto">${dateFormat(commessa.data_esecuzione)}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>articolo</strong></div>
                                <div class="col-md-auto">${commessa.articolo}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>quantità prevista</strong></div>
                                <div class="col-md-auto">${commessa.quantita_prevista}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>quantità prodotta</strong></div>
                                <div class="col-md-auto item-quantita_prodotta">${commessa.quantita_prodotta}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>quantità di scarto</strong></div>
                                <div class="col-md-auto item-scarto">${commessa.quantita_scarto_difettoso + commessa.quantita_scarto_pieno}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>stato commessa</strong></div>
                                <div class="col-md-auto item-stato">${commessa.stato}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div><div class="col-sm-3"><strong>data di consegna</strong></div>
                                <div class="col-md-auto">${dateFormat(commessa.data_consegna)}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>`);
    let table = $('#dt-commesse').DataTable();
    
    // aggiungo il codice generato dinamicamente all'elenco delle commesse
    table.rows.add(template).data(commessa).draw();
    $(`#table_${commessa._id}`).data(commessa);
};

// aggiorno i dati nell'elenco delle commesse
const updateTableRow = (commessa) => {
    commessa.scarto = commessa.quantita_scarto_difettoso + commessa.quantita_scarto_pieno;
    commessa.data_ultima_esecuzione = dateFormat(commessa.data_aggiornamento);

    _.forIn(commessa, (value, key) => {
        if($(`#table_${commessa._id} .item-${key}`)) {
            $(`#table_${commessa._id} .item-${key}`).html(value);
        }
    });
    
    $(`#table_${commessa._id}`).attr('item-date', commessa.data_aggiornamento);
    $(`#table_${commessa._id} .stato-container`).empty();

    if (commessa.stato === "completata") {
        $(`#table_${commessa._id} .stato-container`).prepend('<span class="stato-completato"></span>')
    }
    if (commessa.stato === "fallita") {
        $(`#table_${commessa._id} .stato-container`).prepend('<span class="stato-fallito"></span>')
    }
    if (commessa.stato === "in esecuzione") {
        $(`#table_${commessa._id} .stato-container`).prepend('<span class="spinner-grow text-warning" style="height: 25px; width: 25px;" role="status"></span>')
    }
    $(`#table_${commessa._id}`).data(commessa);
};

const dateFormat = (date) => {
    let newDate = new Date(date);
    let giorni = ["Domenica", "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"]
    //let mesi = ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"]
    let formattedDate = `${giorni[newDate.getDay()]} ${("0" + newDate.getDate()).slice(-2)}/${("0" + (newDate.getMonth() + 1)).slice(-2)}/${newDate.getFullYear()} ${("0" + newDate.getUTCHours()).slice(-2)}:${("0" + newDate.getUTCMinutes()).slice(-2)}:${("0" + newDate.getUTCSeconds()).slice(-2)}`
    return formattedDate;
};
//#endregion

//#region METODI RELATIVI AL GRAFICO

// istanzio il grafico
const renderGraph = () => {
    let optionsLine = {
        chart: {
            width: "95%",
            foreColor: "#f8f9fa",
            //background: "#0e0831",
            toolbar: {
                show: true
            },
            height: "200%",
            type: 'line',
            zoom: {
                enabled: true
            },
            dropShadow: {
                enabled: true,
                top: 3,
                left: 2,
                blur: 4,
                opacity: 1,
            }
        },
        tooltip: {
            theme: "dark"
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        colors: ["#00F790", '#F7AA00'],
        series: [{
            name: "pezzi prodotti",
            data: graphData[1]
        },
        {
            name: "pezzi scartati",
            data: graphData[2]
        }
        ],
        markers: {
            size: 6,
            strokeWidth: 0,
            hover: {
                size: 9
            }
        },
        grid: {
            show: true,
            padding: {
                bottom: 0
            }
        },
        labels: graphData[0],
        xaxis: {
            tooltip: {
                enabled: false
            }
        },
        legend: {
            position: 'bottom',
            horizontalAlign: 'center'
        },
        responsive: [
            {
                breakpoint: 1000,
                options: {
                }
            }
        ]
    }

    let chartLine = new ApexCharts(document.querySelector('#graph'), optionsLine);
    chartLine.render();
};

// metodo per formattare i dati da passare al grafico
const formatGraphData = (dati) => {
    let formattedGraphData = [];

    for (const x of dati) {
        let data_grafico = formattedGraphData[graphDateFormat(x.data_aggiornamento)];
        let pezziTotali = x.quantita_prodotta;
        let pezziScartati = x.quantita_scarto_difettoso + x.quantita_scarto_pieno;

        if (data_grafico == undefined) {
            data_grafico = {
                "pezzi_totali": pezziTotali,
                "pezzi_scartati": pezziScartati
            }
        } else {
            data_grafico = {
                "pezzi_totali": pezziTotali + data_grafico.pezzi_totali,
                "pezzi_scartati": pezziScartati + data_grafico.pezzi_scartati
            }
        }
        formattedGraphData[graphDateFormat(x.data_aggiornamento)] = data_grafico;
    }

    let data_x = [];
    let pezziTotaliGiornalieri = [];
    let pezziScartatiGiornalieri = [];

    Object.keys(formattedGraphData).forEach(function (key) {
        data_x.push(key);
        pezziTotaliGiornalieri.push(formattedGraphData[key].pezzi_totali);
        pezziScartatiGiornalieri.push(formattedGraphData[key].pezzi_scartati);
    });

    return [data_x, pezziTotaliGiornalieri, pezziScartatiGiornalieri];
};

const graphDateFormat = (rawDate) => {
    let newDate = new Date(rawDate);
    let formattedDate = `${newDate.getFullYear()}-${("0" + (newDate.getMonth() + 1)).slice(-2)}-${("0" + newDate.getDate()).slice(-2)}`
    return formattedDate;
};
//#endregion