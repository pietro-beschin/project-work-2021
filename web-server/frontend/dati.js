$(document).ready(() => {
    const urlParams = new URLSearchParams(window.location.search);
    fetchData();
});

const fetchData = (() => {
    $.ajax({
        type: 'GET',
        url: 'backend/src/db.json',
    }).then(result => {
        $('#accordion-commesse').empty();
        result.forEach(commessa => addCommessaToList(commessa));
    });
});

const addCommessaToList = (commessa) => {
    const template = $(`
        <div class="card bg-secondary">
            <div class="card-header" role="tab" id="heading_${commessa.id_commessa}">
                <a data-toggle="collapse" data-parent="#accordion-commesse" href="#collapse_${commessa.id_commessa}" aria-expanded="false" aria-controls="collapse_${commessa.id_commessa}">
                    <div class="h5 text-light ml-3 mt-2">${commessa.commessa}<i class="fas fa-angle-down rotate-icon"></i></div>
                </a>
            </div>
            <div id="collapse_${commessa.id_commessa}" class="collapse bg-light text-dark" role="tabpanel" aria-labelledby="heading_${commessa.id_commessa}" data-parent="accordion-commesse">
                <div class="card-body">
                    <div class="row">
                        <div class="col-2 ml-5">
                            <strong>articolo</strong><br>
                            <strong>codice commessa</strong><br>
                            <strong>quantità prevista</strong><br>
                            <strong>data di consegna</strong><br>
                            <strong>quantità prodotta</strong><br>
                            <strong>quantità di scarto</strong>
                        </div>
                        <div class="col-3">
                            ${commessa.articolo}<br>
                            ${commessa.id_commessa}<br>
                            ${commessa.quantita_prevista}<br>
                            ${dateFormat(commessa.data_consegna)}<br>
                            ${commessa.quantita_prodotta}<br>
                            ${commessa.quantita_scarto}
                        </div>
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