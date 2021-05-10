$(document).ready(() => {
    const urlParams = new URLSearchParams(window.location.search);
    
    fetchData();
});

const fetchData = () => {
    $.ajax({
        type: 'GET',
        url: 'http://localhost:3000/api/history',
    }).then(result => {
        $('#accordion-commesse').empty();
        result.forEach(commessa => addCommessaToList(commessa));
    });
};

const addCommessaToList = (commessa) => {
    const template = $(`
        <div class="card bg-dark">
            <div class="card-header" role="tab" id="heading_${commessa._id}">
                <a data-toggle="collapse" data-parent="#accordion-commesse" href="#collapse_${commessa._id}" aria-expanded="false" aria-controls="collapse_${commessa._id}">
                    <div class="row">
                        <div class="col-md-auto"></div>
                        <div class="col h4 text-light mt-2">${commessa.codice_commessa}<i class="fas fa-angle-down rotate-icon"></i></div>        
                    </div>
                </a>
            </div>
            <div id="collapse_${commessa._id}" class="collapse bg-light text-dark" role="tabpanel" aria-labelledby="heading_${commessa._id}" data-parent="accordion-commesse">
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
        </div>`);
    template.data(commessa);

    $('#accordion-commesse').prepend(template);
};

const datiLavorazione = (commessa) => {
    const template = $(``);
    template.data(commessa);

    $('#card-lavorazione').prepend(template);
};

const datiStato = (commessa) => {
    const template = $(``);
    template.data(commessa);

    $('#card-stato').prepend(template);
};

const datiProgresso = (commessa) => {
    const template = $(``);
    template.data(commessa);

    $('#card-progresso').prepend(template);
};
const datiErrori = (commessa) => {
    const template = $(``);
    template.data(commessa);

    $('#card-errori').prepend(template);
};

const dateFormat = (date) => {
    let newDate = new Date(date);
    let formattedDate = newDate.toUTCString();
    return formattedDate;
}