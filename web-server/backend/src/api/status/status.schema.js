const mongoose = require('mongoose');

let statusSchema = mongoose.Schema({
    id : false,
    versionKey : false,
    stato : {
        type : String,
        required : true
    },
    allarme : String,
    velocita : Number
},
{
    collection : 'status'
});

module.exports = mongoose.model('status', statusSchema);