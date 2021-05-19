const moment = require('moment');
const historySchema = require('./history.schema');

module.exports.list = async (query) => {
        const q = {};
        /*if(query.articolo){
            q.articolo = regExpArticolo(query.articolo);
        }*/
        if(query.from || query.to){
            q.data_consegna = {};
        }
        if(query.from){
            q.data_consegna.$gte = moment(new Date(query.from)/*.setHours(0,0,0,000))*/.utcOffset(0, true));
        }
        if(query.to){
            q.data_consegna.$lte = moment(new Date(query.to)/*.setHours(23,59,59,999))*/.utcOffset(0, true));
        }
        if(query.hideCompleted === 'true'){
            q.$where = "Number(this.quantita_prodotta) < Number(this.quantita_prevista)";
        }
        return await historySchema.find(q, {articolo: {$regex : regExpArticolo(query.articolo)}});
}

module.exports.store = async (data) => {  
    console.log("ricevuto: "+data.codice_commessa);
    if(data.codice_commessa == "" && (ultimaInserita = await historySchema.findOne().sort({'_id' : -1}))){
        let stato = (ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista) ? "completata" : "fallita";
        return await historySchema.findByIdAndUpdate(ultimaInserita._id, {"stato" : stato});
    }

    if(result = await historySchema.findOne({codice_commessa : data.codice_commessa})){   //se esiste già e codice commessa c'è
        return await historySchema.findByIdAndUpdate(result._id, data); //la aggiorno
    }else if(ultimaInserita = await historySchema.findOne().sort({'_id' : -1})){      //modifico precedente
        let stato = (ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista) ? "completata" : "fallita";
        await historySchema.findByIdAndUpdate(ultimaInserita._id, {"stato" : stato});  
    }
}
    
    

/*     if(result = await historySchema.findOne({codice_commessa : data.codice_commessa})){   //se esiste già e codice commessa c'è
        return await historySchema.findByIdAndUpdate(result._id, data); //la aggiorno
    }else if(ultimaInserita = await historySchema.findOne().sort({'_id' : -1})){      //modifico precedente
        let stato = (ultimaInserita.quantita_prodotta >= ultimaInserita.quantita_prevista) ? "completata" : "fallita";
            await historySchema.findByIdAndUpdate(ultimaInserita._id, {"stato" : stato});  
    }

    if(data.codice_commessa != ""){    
        data.stato = 'in esecuzione';   //la creo
        console.log("NUOVO");
        return await historySchema.create(data);
    }
} */

module.exports.getLastCommessa = async () => {
    return await historySchema.findOne().sort({'_id' : -1});
}

module.exports.delete = async (id) => {
    await historySchema.findByIdAndDelete(id);
}

module.exports.clear = async () => {
    await historySchema.remove();
}

function regExpArticolo(value) {
    let parts = value.trim().split(/\W+/);
    const regExpParts = parts.map(t => `(?=.*${t}.*)`);
    return new RegExp(`^${regExpParts.join('')}.+`,'i');
}