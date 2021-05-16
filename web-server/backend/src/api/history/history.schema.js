const mongoose = require('mongoose');
const historyModel = require('./history.model');

let historySchema = mongoose.Schema({
    //fallita : Boolean,
    id : false,
    versionKey : false,
    codice_commessa : String,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto_difettoso : Number,
    quantita_scarto_pieno : Number,
    stato: String
},
{
    collection : 'history'
});

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('history', historySchema);
