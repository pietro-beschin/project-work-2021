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
        res.status(201);
    }catch(err){
        next(err);
    }
}

module.exports.getLastCommessa = async(req, res, next) => {
    try{
        const result = await historyModel.getLastCommessa();
        res.json(result);
    }catch(err){
        throw new Error('Empty DB');
    }
}

module.exports.delete = async (req, res, next) => {
    try{
        await historyModel.delete(req.params.codice_commessa);
        res.json("RECORD CON CODICE_COMMESSA " + req.params.codice_commessa + "ELIMINATO!");
    }catch(err){
        throw new Error('Not Found');
    }
}

module.exports.clear = async (req, res, next) => {
    try{
        await historyModel.clear();
        res.json("HISTORY SVUOTATA!");
    }catch(err){
        next(err);
    }
}