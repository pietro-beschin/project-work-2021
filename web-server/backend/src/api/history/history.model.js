const historySchema = require('./history.schema');
const moment = require('moment');

module.exports.list = async (query) => {
    const q = {};
    if(query.from || query.to){
        q.date={};
    }
    if(query.from){
        q.date.$gte = query.from;
    }
    if(query.to){
        q.date.$lte = query.to;
    }
    if(query.product){
        q.product = query.product;
    }
    if(query.onlyCompleted){
        if(query.onlyCompleted==='true'){
            q.
        }
    }
}