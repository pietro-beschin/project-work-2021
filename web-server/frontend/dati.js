$(document).ready(function () {
    fetchAllData();

    $('#btnCerca').click(() => {
        fetchFilteredData();
    });

    $('#btnClearFilters').click(() => {
        $('#nome-articolo').val('');
        $('#dtp-inizio').val('');
        $('#dtp-fine').val('');
        fetchFilteredData();
    });

    $('input[name="switch-completato"]').change(() => {
        fetchFilteredData();
    });

    /* setInterval(timingLoad, 3000);
    function timingLoad() {
        $.ajax({
            type: 'GET',
            url: `${baseURL}lastCommessaStatus`,
        }).then(commessa => {
            console.log(commessa);
            $('#quadrante-lavorazione').html(`${commessa.history.articolo}`);
            $('#quadrante-stato').html(`${commessa.status.stato}`);
            $('#quadrante-progresso').html(`${commessa.history.quantita_prodotta}/${commessa.history.quantita_prevista}`);
            $('#quadrante-progresso-percentuale').html(`${((commessa.history.quantita_prodotta * 100) / commessa.history.quantita_prevista).toFixed(1)}%`);
            $('#quadrante-allarmi').html(`${commessa.status.allarme}`);
        });
    } */
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
        $.ajax({
            type: 'GET',
            url: `${baseURL}history`,
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

const loadTable = (function () {
    let table = $('#dt-commesse');
    let rowCount = table.DataTable().data().count()

    table.DataTable().destroy()
    table.DataTable({
        "serverSide": false,
        "iDisplayLength": 10,
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
        url: `${baseURL}historyStatus`,
    })
        .then(result => {
            $('#accordion-commesse').empty();
            result.history.forEach(commessa => {
                if ($('#switch-completato').is(":checked") === true) {
                    if (commessa.history.stato === 'fallita') {
                        addCommessaToList(commessa)
                    }
                } else {
                    addCommessaToList(commessa)
                }
            });
            datiStato(result);
            datiErrori(result);
            datiLavorazione(result);
            datiProgresso(result);
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
    let mesi = ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"]
    let formattedDate = `${giorni[newDate.getDay()]} ${("0" + newDate.getDate()).slice(-2)} ${mesi[newDate.getMonth()]} ${newDate.getFullYear()} ${("0" + newDate.getUTCHours()).slice(-2)}:${("0" + newDate.getUTCMinutes()).slice(-2)}:${("0" + newDate.getUTCSeconds()).slice(-2)}`
    return formattedDate;
}