let datiCommesse;
$(document).ready(function () {
    renderGraph()
    initTable();
    fetchAllData();
    //$('#div-nessun-risultato').show()

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
    });

    setInterval(statusPolling, 1000);
    function statusPolling() {
        $.ajax({
            type: 'GET',
            url: `${baseURL}lastCommessaStatus`,
        }).then(commessa => {
            /* $('#quadrante-lavorazione').html(`${commessa.history.articolo}`);
            $('#quadrante-stato').html(`${commessa.status.stato}`);
            $('#quadrante-progresso').html(`${commessa.history.quantita_prodotta}/${commessa.history.quantita_prevista}`);
            $('#quadrante-progresso-percentuale').html(`${((commessa.history.quantita_prodotta * 100) / commessa.history.quantita_prevista).toFixed(1)}%`);
            $('#quadrante-allarmi').html(`${commessa.status.allarme}`); */
            updateStatus(commessa);
        });
    }

    setInterval(historyPolling, 2000);
    function historyPolling() {
        fetchNewCommesse();
    }
});

const baseURL = 'http://54.85.250.76:3000/api/';
let nomeArticolo;
let graphData;
let chartLine;

const toggle404 = () => {

}


const fetchAllData = () => {
    $.ajax({
        type: 'GET',
        url: `${baseURL}historyStatus`,
    }).then(result => {
        datiCommesse = result.history;

        $('#accordion-commesse').empty();
        result.history.forEach(commessa => {
            if ($('#switch-completato').is(":checked") === true) {
                if (commessa.stato === 'fallita') {
                    addCommessaToList(commessa)
                }
            } else {
                addCommessaToList(commessa)
            }
            updateTableRow(commessa);
        });
        //datiStato(result);
        //datiErrori(result);
        //datiLavorazione(result);
        //datiProgresso(result);

        updateStatus(result);

        let table = $('#dt-commesse').DataTable();
        table.rows().invalidate().draw(true);
        
        graphData = formatGraphData(result.history);
        renderGraph();
        chartLine.updateSeries([{
            name: "pezzi prodotti",
            data: graphData[1]
        },
        {
            name: "pezzi scartati",
            data: graphData[2]
        }])
        chartLine.updateOptions({
            labels: graphData[0],
        })
    });
};

const fetchNewCommesse = () => {
    nomeArticolo = document.getElementById("nome-articolo").value;
    let startDate = $("#dtp-inizio").val();
    let endDate = $("#dtp-fine").val();

    if (event && event.preventDefault) {
        event.preventDefault();
    }

    $.ajax({
        type: 'GET',
        url: `${baseURL}history?articolo=${nomeArticolo}&from=${startDate}&to=${endDate}`,
    }).then(result => {
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
        
        if(!result.length) {
            $('#dt-commesse_wrapper').hide()
            $('#div-nessun-risultato').show()
        } else {
            $('#div-nessun-risultato').hide()
            $('#dt-commesse_wrapper').show()
        }

        datiCommesse = result;
    });
};

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
    }

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

const updateStatus = (commessa) => {
    $('#quadrante-lavorazione').html(`${commessa.history.articolo}`);
    $('#quadrante-stato').html(`${commessa.status.stato}`);
    $('#quadrante-progresso').html(`${commessa.history.quantita_prodotta}/${commessa.history.quantita_prevista}`);
    $('#quadrante-progresso-percentuale').html(`${((commessa.history.quantita_prodotta * 100) / commessa.history.quantita_prevista).toFixed(1)}%`);
    $('#quadrante-allarmi').html(`${commessa.status.allarme}`);

    if (commessa.history.stato === "completata") {
        $('#indicatore-stato').toggleClass("stato-completato")
    }
    if (commessa.history.stato === "fallita") {
        $('#indicatore-stato').prepend('<span class="stato-fallito"></span>')
    }
    if (commessa.history.stato === "in esecuzione") {
        $('#indicatore-stato').prepend('<span class="spinner-border text-warning" role="status"></span>')
    }
}

const updateTableRow = (commessa) => {
    commessa.scarto = commessa.quantita_scarto_difettoso + commessa.quantita_scarto_pieno;
    commessa.data_di_esecuzione = dateFormat(commessa.data_esecuzione);

    _.forIn(commessa, (value, key) => {
        if($(`#table_${commessa._id} .item-${key}`)) {
            $(`#table_${commessa._id} .item-${key}`).html(value);
        }
    });
    
    $(`#table_${commessa._id}`).attr('item-date', commessa.data_esecuzione);
    $(`#table_${commessa._id} .stato-container`).empty();

    if (commessa.stato === "completata") {
        $(`#table_${commessa._id} .stato-container`).prepend('<span class="stato-completato"></span>')
    }
    if (commessa.stato === "fallita") {
        $(`#table_${commessa._id} .stato-container`).prepend('<span class="stato-fallito"></span>')
    }
    if (commessa.stato === "in esecuzione") {
        $(`#table_${commessa._id} .stato-container`).prepend('<span class="spinner-border text-warning" style="height: 25px; width: 25px;" role="status"></span>')
    }
    $(`#table_${commessa._id}`).data(commessa);
}

const addCommessaToList = (commessa) => {
    const template = $(`
    <tr class="table-item">
        <td>
            <div id="table_${commessa._id}" class="accordion md-accordion" role="tablist" item-date="${commessa.data_esecuzione}">
                <div class="card lighter-back border-lighter">
                    <div class="card-header" role="tab" id="heading_${commessa._id}">
                        <a data-toggle="collapse" data-parent="#accordion-commesse" href="#collapse_${commessa._id}" aria-expanded="false" aria-controls="collapse_${commessa._id}">
                            <div class="row">
                                <div class="stato-container col-md-auto mt-2"></div>
                                <div class="col-lg-7 h4 text-light mt-2">${commessa.codice_commessa}</div>
                                <div class="col-md-auto text-light mt-2 item-data_di_esecuzione">${dateFormat(commessa.data_esecuzione)}</div>       
                            </div>
                        </a>
                    </div>
                    <div id="collapse_${commessa._id}" class="collapse med-back" role="tabpanel" aria-labelledby="heading_${commessa._id}" data-parent="accordion-commesse">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-auto"></div><div class="col-sm-3"><strong>data ultima esecuzione</strong></div>
                                <div class="item-data-esecuzione col-md-auto item-data_di_esecuzione">${dateFormat(commessa.data_esecuzione)}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>articolo</strong></div>
                                <div class="col-md-auto">${commessa.articolo}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>codice commessa</strong></div>
                                <div class="col-md-auto">${commessa.codice_commessa}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>stato commessa</strong></div>
                                <div class="col-md-auto item-stato">${commessa.stato}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>quantità prevista</strong></div>
                                <div class="col-md-auto">${commessa.quantita_prevista}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div><div class="col-sm-3"><strong>data di consegna</strong></div>
                                <div class="col-md-auto">${dateFormat(commessa.data_consegna)}</div>
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
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>`);
    let table = $('#dt-commesse').DataTable();
    
    table.rows.add(template).data(commessa).draw();
    $(`#table_${commessa._id}`).data(commessa);
};

const datiLavorazione = (commessa) => {
    const template = $(`
    <h1 id="quadrante-lavorazione" class="display-5">${commessa.history[commessa.history.length - 1].articolo}</h1>
    <hr>
    <div class="lead">prodotto in lavorazione</div>
    `);
    template.data(commessa);

    $('#card-lavorazione').prepend(template);
};

const datiStato = (commessa) => {
    const template = $(`
    <div class="row d-flex justify-content-between">
        <div class="col-sm-auto">
            <h1 id="quadrante-stato" class="display-5">${commessa.status.stato}</h1>
        </div>
        <div class="col-sm-auto mt-3" id="indicatore-stato">
            
        </div>
    </div>
    <hr>
    <div class="lead">stato</div>`);

    template.data(commessa);

    $('#card-stato').prepend(template);

    if (commessa.history.stato === "completata") {
        $('#indicatore-stato').prepend('<span class="stato-completato"></span>')
    }
    if (commessa.history.stato === "fallita") {
        $('#indicatore-stato').prepend('<span class="stato-fallito"></span>')
    }
    if (commessa.history.stato === "in esecuzione") {
        $('#indicatore-stato').prepend('<span class="spinner-border text-warning" role="status"></span>')
    }
};

const datiProgresso = (commessa) => {
    let pos = commessa.history.length - 1;

    const template = $(`
    <div class="row d-flex justify-content-between">
        <div id="div-progresso" class="col">
            <h1 id="quadrante-progresso" class="display-5">${commessa.history[pos].quantita_prodotta}/${commessa.history[pos].quantita_prevista}</h1>
        </div>
        <div class="col">
            <h1 id="quadrante-progresso-percentuale" class="display-7">${((commessa.history[pos].quantita_prodotta * 100) / commessa.history[pos].quantita_prevista).toFixed(1)}%</h1>
        </div>
    </div>
    <hr>
    <div class="lead">pezzi prodotti</div>`);
    template.data(commessa);

    $('#card-progresso').prepend(template);
};
const datiErrori = (commessa) => {
    const template = $(`
    <h1 id="quadrante-allarmi" class="display-6">${commessa.status.allarme}</h1>
    <hr>
    <div class="lead">errori</div>`);
    template.data(commessa);

    $('#card-errori').prepend(template);
};

const dateFormat = (date) => {
    let newDate = new Date(date);
    let giorni = ["Domenica", "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"]
    //let mesi = ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"]
    let formattedDate = `${giorni[newDate.getDay()]} ${("0" + newDate.getDate()).slice(-2)}/${("0" + (newDate.getMonth() + 1)).slice(-2)}/${newDate.getFullYear()} ${("0" + newDate.getUTCHours()).slice(-2)}:${("0" + newDate.getUTCMinutes()).slice(-2)}:${("0" + newDate.getUTCSeconds()).slice(-2)}`
    return formattedDate;
}

const formatGraphData = (dati) => {
    let formattedGraphData = [];

    for (const x of dati) {
        let data_grafico = formattedGraphData[graphDateFormat(x.data_esecuzione)];
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
        formattedGraphData[graphDateFormat(x.data_esecuzione)] = data_grafico;
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
}

const graphDateFormat = (rawDate) => {
    let newDate = new Date(rawDate);
    let formattedDate = `${newDate.getFullYear()}-${("0" + (newDate.getMonth() + 1)).slice(-2)}-${("0" + newDate.getDate()).slice(-2)}`
    return formattedDate;
}

const renderGraph = () => {
    let optionsLine = {
        chart: {
            width: "95%",
            foreColor: "#f8f9fa",
            background: "#0e0831",
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
        series: [/* {
            name: "pezzi prodotti",
            data: graphData[1]
        },
        {
            name: "pezzi scartati",
            data: graphData[2]
        } */
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
        //labels: graphData[0],
        noData: {
            text: 'Nessun dato da visualizzare'
        },
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
}