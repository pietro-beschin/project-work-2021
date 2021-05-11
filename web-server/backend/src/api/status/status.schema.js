const mongoose = require('mongoose');

let statusSchema = mongoose.Schema({
    attiva : Boolean,
    progresso_lavorazione : Number,
    stato : String,
    velocita : Number
},
{
    collection : 'status'
});

module.exports = mongoose.model('status', statusSchema);