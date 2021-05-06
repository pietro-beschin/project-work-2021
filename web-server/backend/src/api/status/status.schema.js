const mongoose = require('mongoose');

let statusSchema = mongoose.Schema({
    lavorazione : String,
    attiva : Boolean,
    progresso_lavorazione : Number,
    errori : [String]
},
{
    collection : 'status'
});

module.exports = mongoose.model('status', statusSchema);