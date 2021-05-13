const mongoose = require('mongoose');
const async = require('async');


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
    .set(async () => {
        if(this.quantita_prodotta < this.quantita_prevista){
            if(this._id === await historySchema.findOne()){
                console.log(await historySchema.findOne());
                this.set('non completata');
            }
            this.set('fallita');
        }
        this.set('completata');
    });

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('history', historySchema);
