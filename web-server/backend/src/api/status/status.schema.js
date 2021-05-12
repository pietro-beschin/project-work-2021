const mongoose = require('mongoose');
const historyModel = require('../history/history.model');

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

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

statusSchema.virtual('progresso_lavorazione')
    .get(function() {
        const lastElement = await historyModel.getLastCommessa();
        console.log(lastElement.codice_commessa);
        return (lastElement.quantita_prodotta / lastElement.quantita_prevista) * 100;
    });


module.exports = mongoose.model('status', statusSchema);