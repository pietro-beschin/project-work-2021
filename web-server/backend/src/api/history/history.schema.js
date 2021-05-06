const mongoose = require('mongoose');


let historySchema = mongoose.Schema({
    codice_commessa : String,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto : Number
    //quantita_scarto_difettosa : Number,
    //quantita_scarto_pieno : Number,


},
{
    collection : 'history'
});

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

historySchema.virtual('completed')
    .get(function() {
        return this.quantita_prodotta >= this.quantita_prevista;
    });

module.exports = mongoose.model('history', historySchema);
