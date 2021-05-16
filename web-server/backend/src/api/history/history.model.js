const moment = require('moment');
const historySchema = require('./history.schema');

module.exports.list = async (query) => {
        const q = {};
        if(query.articolo){
            q.articolo = query.articolo;
        }
        if(query.from || query.to){
            q.data_consegna = {};
        }
        if(query.from){
            q.data_consegna.$gte = moment(new Date(query.from).setHours(0,0,0,000)).utcOffset(0, true);
        }
        if(query.to){
            q.data_consegna.$lte = moment(new Date(query.to).setHours(23,59,59,999)).utcOffset(0, true);
        }
        if(query.hideCompleted === 'true'){
            q.$where = "Number(this.quantita_prodotta) < Number(this.quantita_prevista)";
        }
        return await historySchema.find(q);
}

module.exports.store = async (data) => {
    if(result = await historySchema.findOne({codice_commessa : data.codice_commessa })){    //se esista già
        //la aggiorno
        if(this.quantita_prodotta >= this.quantita_prevista){
            data.stato = 'completata';
            return await historySchema.findByIdAndUpdate(result._id, data);
        }
    }else{
        console.log("L'ID DI RESULT E' "+result._id);
        let ultimaInserita = await historySchema.findOne();
        if(ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista){
            await historySchema.findByIdAndUpdate(ultimaInserita._id, {'stato' : 'completata'});
        }else{
            await historySchema.findByIdAndUpdate(ultimaInserita._id, {'stato' : 'fallita'});
        }
    }
    //la creo
    data.stato = 'in esecuzione';
    return await historySchema.create(data);
}

module.exports.getLastCommessa = async () => {
    return await historySchema.findOne().sort({'_id' : -1});
}