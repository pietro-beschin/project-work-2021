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
    quantita_scarto_pieno : Number
},
{
    collection : 'history'
});

const lastInserted = async () => {
    return await this.findOne();
}


historySchema.virtual('completed')
    .get(function() {
        console.log(lastInserted._id);
        if(this.quantita_prodotta < this.quantita_prevista){
            if(this._id == lastInserted._id){
                return "in esecuzione";
            }
            return "fallita";
        }
        return "completata";
    });

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('history', historySchema);
