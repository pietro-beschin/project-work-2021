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

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('status', statusSchema);