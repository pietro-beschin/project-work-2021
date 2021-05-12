const historyModel = require('./history.model');

module.exports.list = async (req, res, next) => {
    try{
        const list = await historyModel.list(req.query);
        res.json(list);
    }catch(err){
        next(err);
    }
}

module.exports.store = async (req, res, next) => {
    try{
        const result = await historyModel.store(req.body);
        res.json(result);
    }catch(err){
        next(err);
    }
}

module.exports.getLastCommessa = async(req, res, next) => {
    try{
        return await historyModel.getLastCommessa();
    }catch(err){
        next(err);
    }
}