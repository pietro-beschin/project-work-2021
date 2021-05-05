const mongoose = require('mongoose');

const prodottiSchema = mongoose.Schema({
    id_commessa : Number,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto : Number
});

module.exports = mongoose.model('prodotti', prodottiSchema);
