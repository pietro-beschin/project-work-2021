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
        const result = await historyModel.getLastCommessa();
        res.json(result);
    }catch(err){
        next(err);
    }
}

module.exports.delete = async (req, res, next) => {
    try{
        await historyModel.delete(req.params.id);
        res.json("RECORD CON ID " + req.params.id + "ELIMINATO!");
    }catch(err){
        res.json("RECORD NON TROVATO");
        next(err);
    }
}

module.exports.clear = async (req, res, next) => {
    try{
        await historyModel.clear();
        res.json("HISTORY SVUOTATA!");
    }catch(err){
        res.json("SVUOTAMENTO NON RIUSCITO");
        next(err);
    }
}