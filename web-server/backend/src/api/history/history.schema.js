const mongoose = require('mongoose');
const historyModel = require('./history.model');

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
    quantita_scarto_pieno : Number,
    completed: String
},
{
    collection : 'history'
});

async function lastInserted() {
    return await historySchema.findOne();
}

historySchema.virtual('completed')
    .get(async function() {
        const last = await historyModel.getFirstId();
        console.log(last);
        if(this.quantita_prodotta < this.quantita_prevista){
            if(this._id == last){
                return "in esecuzione";
            }
            return "fallita";
        }
        return "completata";
    });

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('history', historySchema);
