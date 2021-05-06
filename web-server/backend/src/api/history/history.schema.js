const mongoose = require('mongoose');


let historySchema = mongoose.Schema({
    codice_commessa : String,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto : Number,
},
{
    toJSON: {virtuals: true},
    toObject: {virtuals: true}
},
{
    collection : 'history'
});

historySchema.virtual('completed')
    .get(function() {
        return this.quantita_prodotta >= this.quantita_prevista;
    });

module.exports = mongoose.model('history', historySchema);
