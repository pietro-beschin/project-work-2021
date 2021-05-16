$(document).ready(function () {
    //const urlParams = new URLSearchParams(window.location.search);

    fetchStatus();
    fetchCurrentData();
    fetchAllData();

    $('#btnCerca').click(() => {
        fetchFilteredData();
    });

    $('input[name="switch-completato"]').change(() => {
        fetchFilteredData();
    })
});

//const baseURL = 'http://localhost:3000/api/';
const baseURL = 'http://54.85.250.76:3000/api/';

const fetchFilteredData = () => {
    let nomeArticolo = $("#nome-articolo").val();
    let startDate = $("#dtp-inizio").val();
    let endDate = $("#dtp-fine").val();

    $('#dt-commesse').DataTable().destroy()

    event.preventDefault();

    if (!nomeArticolo && !startDate && !endDate) {
        fetchAllData();
    };
    if (!nomeArticolo) {
        $.ajax({
            type: 'GET',
            url: `${baseURL}history?from=${startDate}&to=${endDate}`,
        }).then(result => {
            $('#accordion-commesse').empty();
            result.forEach(commessa => {
                if ($('#switch-completato').is(":checked") === true) {
                    if (commessa.stato === 'fallita') {
                        addCommessaToList(commessa)
                    }
                } else {
                    addCommessaToList(commessa)
                }
            });
            loadTable();
        });
    }
    if (!startDate && !endDate) {
        $.ajax({
            type: 'GET',
            url: `${baseURL}history?articolo=${nomeArticolo}`,
        }).then(result => {
            $('#accordion-commesse').empty();
            result.forEach(commessa => {
                if ($('#switch-completato').is(":checked") === true) {
                    if (commessa.stato === 'fallita') {
                        addCommessaToList(commessa)
                    }
                } else {
                    addCommessaToList(commessa)
                }
            });
            loadTable();
        });
    }
    if (nomeArticolo && startDate && endDate) {
        $.ajax({
            type: 'GET',
            url: `${baseURL}history?articolo=${nomeArticolo}&from=${startDate}&to=${endDate}`,
        }).then(result => {
            $('#accordion-commesse').empty();
            result.forEach(commessa => {
                if ($('#switch-completato').is(":checked") === true) {
                    if (commessa.stato === 'fallita') {
                        addCommessaToList(commessa)
                    }
                } else {
                    addCommessaToList(commessa)
                }
            });
            loadTable();
        });
    };
};

const fetchStatus = () => {
    $.ajax({
        type: 'GET',
        url: `${baseURL}status`,
    }).then(result => {
        datiStato(result);
        datiErrori(result);
    });
}

const fetchCurrentData = () => {
    $.ajax({
        type: 'GET',
        url: `${baseURL}lastCommessa`,
    }).then(result => {
        datiLavorazione(result);
        datiProgresso(result);
    });
};

const loadTable = (function () {
    let table = $('#dt-commesse');
    let rowCount = table.DataTable().data().count()

    table.DataTable().destroy()
    table.DataTable({
        "serverSide": false,
        "iDisplayLength": 5,
        "paging": true,
        //"lengthChange": true,
        "lengthChange": (function () {
            if (rowCount == 0) {
                return false;
            } else {
                return true;
            };
        }),
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
            "emptyTable": "Nessun risultato",
            "paginate": {
                "first": "Prima",
                "last": "Ultima",
                "next": ">",
                "previous": "<"
            }
        }
    });

    //console.log(rowCount)
});

const fetchAllData = () => {
    $.ajax({
        type: 'GET',
        url: `${baseURL}history`,
    })
        .then(result => {
            $('#accordion-commesse').empty();
            result.forEach(commessa => {
                if ($('#switch-completato').is(":checked") === true) {
                    if (commessa.stato === 'fallita') {
                        addCommessaToList(commessa)
                    }
                } else {
                    addCommessaToList(commessa)
                }
            });
            loadTable();
        });
};

const addCommessaToList = (commessa) => {
    const template = $(`
    <tr>
        <td>
            <div class="accordion md-accordion" role="tablist">
                <div class="card lighter-back border-lighter">
                    <div class="card-header" role="tab" id="heading_${commessa._id}">
                        <a data-toggle="collapse" data-parent="#accordion-commesse" href="#collapse_${commessa._id}" aria-expanded="false" aria-controls="collapse_${commessa._id}">
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-lg-7 h4 text-light mt-2">${commessa.codice_commessa}</div>
                                <div class="col-md-auto text-light mt-2">${dateFormat(commessa.data_consegna)}</div>       
                            </div>
                        </a>
                    </div>
                    <div id="collapse_${commessa._id}" class="collapse med-back" role="tabpanel" aria-labelledby="heading_${commessa._id}" data-parent="accordion-commesse">
                        <div class="card-body">
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
                                <div class="col-md-auto">${commessa.quantita_prodotta}</div>
                            </div>
                            <div class="row">
                                <div class="col-md-auto"></div>
                                <div class="col-sm-3"><strong>quantità di scarto</strong></div>
                                <div class="col-md-auto">${commessa.quantita_scarto_difettoso + commessa.quantita_scarto_pieno}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>`);
    template.data(commessa);

    $('#accordion-commesse').append(template);
};

const datiLavorazione = (commessa) => {
    const template = $(`
    <h1 class="display-5">${commessa.articolo}</h1>
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
            <h1 class="display-5">${commessa.stato}</h1>
        </div>
        <div class="col-sm-auto mt-3" id="indicatore-stato">
            
        </div>
    </div>
    <hr>
    <div class="lead">stato</div>`);

    template.data(commessa);

    $('#card-stato').prepend(template);

    if (commessa.stato === "completata") {
        $('#indicatore-stato').prepend('<span class="stato-completato"></span>')
    }
    if (commessa.stato === "fallita") {
        $('#indicatore-stato').prepend('<span class="stato-fallito"></span>')
    }
    if (commessa.stato === "in esecuzione") {
        $('#indicatore-stato').prepend('<span class="spinner-border text-warning" role="status"></span>')
    }
};

const datiProgresso = (commessa) => {
    const template = $(`
    <div class="row d-flex justify-content-between">
        <div class="col">
            <h1 class="display-5">${commessa.quantita_prodotta}/${commessa.quantita_prevista}</h1>
        </div>
        <div class="col">
            <h1 class="display-7">${((commessa.quantita_prodotta * 100) / commessa.quantita_prevista).toFixed(1)}%</h1>
        </div>
    </div>
    <hr>
    <div class="lead">pezzi prodotti</div>`);
    template.data(commessa);

    $('#card-progresso').prepend(template);
};
const datiErrori = (commessa) => {
    const template = $(`
    <h1 class="display-6">${commessa.allarme}</h1>
    <hr>
    <div class="lead">errori</div>`);
    template.data(commessa);

    $('#card-errori').prepend(template);
};

const dateFormat = (date) => {
    let newDate = new Date(date);
    let giorni = ["Domenica", "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"]
    let mesi = ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"]
    let formattedDate = `${giorni[newDate.getDay()]} ${("0" + newDate.getDate()).slice(-2)} ${mesi[newDate.getMonth()]} ${newDate.getFullYear()} ${("0" + newDate.getUTCHours()).slice(-2)}:${("0" + newDate.getUTCMinutes()).slice(-2)}:${("0" + newDate.getUTCSeconds()).slice(-2)}`
    return formattedDate;
}