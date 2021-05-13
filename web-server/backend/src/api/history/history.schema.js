const mongoose = require('mongoose');
const historyModel = require('./history.model');
const lastInsertedId = await (historyModel.getLastCommessa)._id;

let historySchema = mongoose.Schema({
    //fallita : Boolean,
    id : false,
    versionKey : false,
    codice_commessa : String,
    articolo : String,
    quantita_prevista : Number,
    data_consegna : Date,
    quantita_prodotta : Number,
    quantita_scarto_difettoso : Number,
    quantita_scarto_pieno : Number
},
{
    collection : 'history'
});



historySchema.virtual('completed')
    .get(async function() {
        console.log(lastInsertedId);
        if(this.quantita_prodotta < this.quantita_prevista){
            if(this._id == lastInsertedId){
                return "in esecuzione";
            }
            return "fallita";
        }
        return "completata";
    });

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('history', historySchema);
