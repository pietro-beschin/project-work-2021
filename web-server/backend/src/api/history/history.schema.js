const mongoose = require('mongoose');


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
    .get(async () => {
        if(this.quantita_prodotta < this.quantita_prevista){
            if(this._id === await historySchema.findOne()._id){
                return 'non completata';
            }else{
                return 'fallita';
            }
        }else{
            return 'completata';
        }
    });

mongoose.set('toJSON', {virtuals: true});
mongoose.set('toObject', {virtuals: true});

module.exports = mongoose.model('history', historySchema);
