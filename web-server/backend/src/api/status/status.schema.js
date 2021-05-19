const mongoose = require('mongoose');

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

module.exports = mongoose.model('status', statusSchema);