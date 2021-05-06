const mongoose = require('mongoose');

let statusSchema = mongoose.Schema({
    lavorazione : String,
    attiva : Boolean,
    progresso_lavorazione : Number,
    errori : [String],
    velocita : Number,
    avviso_da_op : String
},
{
    collection : 'status'
});

module.exports = mongoose.model('status', statusSchema);