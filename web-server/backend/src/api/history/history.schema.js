const mongoose = require('mongoose');

const storicoSchema = mongoose.Schema({
    _id : Number,       //id_commessa
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto : Number,
},
{
    toJSON: {virtuals: true},
    toObject: {virtuals: true}
});

storicoSchema.virtual('completed')
    .get(function() {
        return this.quantita_prevista > this.quantita_prodotta;
    });

module.exports = mongoose.model('storico', storicoSchema);
