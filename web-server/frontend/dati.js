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
        <div class="card bg-secondary">
            <div class="card-header" role="tab" id="heading_${commessa.id_commessa}">
                <a data-toggle="collapse" data-parent="#accordion-commesse" href="#collapse_${commessa.id_commessa}" aria-expanded="false" aria-controls="collapse_${commessa.id_commessa}">
                    <div class="h5 text-light ml-3 mt-2">${commessa.articolo}<i class="fas fa-angle-down rotate-icon"></i></div>
                </a>
            </div>
            <div id="collapse_${commessa.id_commessa}" class="collapse bg-light text-dark" role="tabpanel" aria-labelledby="heading_${commessa.id_commessa}" data-parent="accordion-commesse">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-auto"></div>
                        <div class="col-sm-3"><strong>articolo</strong></div>
                        <div class="col-md-auto">${commessa.articolo}</div>
                    </div>
                    <div class="row">
                        <div class="col-md-auto"></div>
                        <div class="col-sm-3"><strong>codice commessa</strong></div>
                        <div class="col-md-auto">${commessa.id_commessa}</div>
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
                        <div class="col-md-auto">${commessa.quantita_scarto}</div>
                    </div>
                </div>
            </div>
        </div>`);
    template.data(commessa);

    $('#accordion-commesse').prepend(template);
};

const dateFormat = (date) => {
    let newDate = new Date(date);
    let formattedDate = newDate.toUTCString();
    return formattedDate;
}