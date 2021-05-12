const mongoose = require('mongoose');
const historySchema = require('../history/history.schema');

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
        const lastElement = historySchema.findOne();
        console.log(lastElement.codice_commessa);
        return (lastElement.quantita_prodotta / lastElement.quantita_prevista) * 100;
    });


module.exports = mongoose.model('status', statusSchema);