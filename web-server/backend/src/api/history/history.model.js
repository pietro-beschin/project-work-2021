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
    if(query.showCompleted === 'false'){
        q.completed = {$ne: true};
    }
    return await historySchema.find(q);
}