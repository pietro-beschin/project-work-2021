const statusModel = require('./status.model');

module.exports.getStatus = async (req, res, next) => {
    try{
        let queried = await statusModel.getStatus();
        res.json(queried);
    }catch(err){
        next(err);
    }
}

module.exports.store = async (req, res, next) => {
    try{
        const queried = await statusModel.store(req.body);
        res.json(queried);
        res.status(201);
    }catch(err){
        next(err);
    }  
}