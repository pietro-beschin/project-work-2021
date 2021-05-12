const moment = require('moment');
const { find, findByIdAndUpdate } = require('./history.schema');
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
    let created;
    if(result = await historySchema.findOne({codice_commessa : data.codice_commessa })){
        created = await findByIdAndUpdate(result._id, data);
        console.log("ID DEL RESULT: " + result._id);
    }
    created = await historySchema.create(data);
    return created;
}

module.exports.getLastCommessa = async () => {
    return await historySchema.findOne().sort({'_id' : -1});
}