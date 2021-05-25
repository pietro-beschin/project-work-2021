const moment = require('moment');
const historySchema = require('./history.schema');

module.exports.list = async (query) => {
        const q = {};
        if(query.articolo){
            q.articolo = regExpArticolo(query.articolo);
        }
        if(query.from || query.to){
            q.data_esecuzione = {};
        }
        if(query.from){
            q.data_esecuzione.$gte = moment(new Date(query.from)).utcOffset(0, true);
        }
        if(query.to){
            q.data_esecuzione.$lte = moment(new Date(query.to)).utcOffset(0, true);
        }
        if(query.hideCompleted === 'true'){
            q.$where = "Number(this.quantita_prodotta) < Number(this.quantita_prevista)";
        }
        return await historySchema.find(q);
}

module.exports.store = async (data) => {            //memorizza dati su mongodb
    /* console.log("ricevuto: " + data.codice_commessa);
    let data_aggiornamento = moment(new Date()).utcOffset(-2, true);
    if(data.codice_commessa == "" && (ultimaInserita = await historySchema.findOne().sort({'_id' : -1}))){
        let stato = (ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista) ? "completata" : "fallita";
        
        return await historySchema.findByIdAndUpdate(ultimaInserita._id, {"stato" : stato}, {"data_aggiornamento" : data_aggiornamento});
    }

    if(result = await historySchema.findOne({codice_commessa : data.codice_commessa})){   //se esiste già e codice commessa c'è
        data.data_aggiornamento = moment(new Date()).utcOffset(-2, true);
        return await historySchema.findByIdAndUpdate(result._id, data, {"data_aggiornamento" : data_aggiornamento}); //la aggiorno
    }else if(ultimaInserita = await historySchema.findOne().sort({'_id' : -1})){      //modifico precedente
        let stato = (ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista) ? "completata" : "fallita";
        let data_aggiornamento = moment(new Date()).utcOffset(-2, true);
        await historySchema.findByIdAndUpdate(ultimaInserita._id, {"stato" : stato}, {"data_aggiornamento" : data_aggiornamento});  
    }
    data.data_aggiornamento = moment(new Date()).utcOffset(-2, true);
    data.stato = 'in esecuzione';
    console.log("NUOVO");
    return await historySchema.create(data);    //la creo
} */


    data.data_aggiornamento = moment(new Date()).utcOffset(-2, true);
    if(result = await historySchema.findOne({codice_commessa : data.codice_commessa})){   //se esiste già e codice commessa c'è
        return await historySchema.findByIdAndUpdate(result._id, data); //la aggiorno
    }else if(ultimaInserita = await historySchema.findOne().sort({'_id' : -1})){      //modifico precedente
        let stato = (ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista) ? "completata" : "fallita";
        await historySchema.findByIdAndUpdate(ultimaInserita._id, {"stato" : stato}, {"data_aggiornamento" : data.data_aggiornamento});  
    }

    if(data.codice_commessa != ""){
        data.data_aggiornamento = moment(new Date()).utcOffset(-2, true);    
        data.stato = 'in esecuzione';   //la creo
        console.log("NUOVO");
        return await historySchema.create(data);
    }
}

module.exports.getLastCommessa = async () => {
    return await historySchema.findOne().sort({'_id' : -1});
}

module.exports.delete = async (commessa) => {
    await historySchema.remove({"codice_commessa" : commessa});
}

module.exports.clear = async () => {
    await historySchema.remove();
}

function regExpArticolo(value) {
    let parts = value.trim().split(/\W+/);
    const regExpParts = parts.map(t => `(?=.*${t}.*)`);
    return new RegExp(`^${regExpParts.join('')}.+`,'i');
}