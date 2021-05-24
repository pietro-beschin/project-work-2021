const mongoose = require('mongoose');
const historyModel = require('./history.model');

let historySchema = mongoose.Schema({
    id : false,
    versionKey : false,
    codice_commessa : String,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    data_aggiornamento : Date,
    data_fine : Date,
    quantita_prodotta : Number,
    quantita_scarto_difettoso : Number,
    quantita_scarto_pieno : Number,
    stato: {
        type : String,
        enum : ['completata', 'fallita', 'in esecuzione']
    }
},
{
    collection : 'history'
});

module.exports = mongoose.model('history', historySchema);
