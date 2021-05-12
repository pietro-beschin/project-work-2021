const mongoose = require('mongoose');
const historyModel = require('../history/history.model');
const async = require('async');

let statusSchema = mongoose.Schema({
    id : false,
    versionKey : false,
    stato : String,
    allarme : String,
    velocita : Number
},
{
    collection : 'status'
});



statusSchema.virtual('progresso_lavorazione')
    .get (async function() {
        const lastElement = await historyModel.getLastCommessa();
        console.log(lastElement.codice_commessa);
        return (lastElement.quantita_prodotta / lastElement.quantita_prevista) * 100;
    });

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('status', statusSchema);