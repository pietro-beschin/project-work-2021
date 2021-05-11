const mongoose = require('mongoose');
const historySchema = require('../history/history.schema');

let statusSchema = mongoose.Schema({
    stato : String,
    allarme : String,
    velocita : Number
},
{
    collection : 'status'
});

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

historySchema.virtual('progresso_lavorazione')
    .get(function() {
        const lastElement = historySchema.findOne();
        return (lastElement.quantita_prodotta / lastElement.quantita_prevista) * 100;
    });


module.exports = mongoose.model('status', statusSchema);