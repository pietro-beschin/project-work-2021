const statusModel = require('./status.model');

module.exports.getStatus = async (req, res, next) => {
    try{
        const result = await statusModel.getStatus();
        console.log(result);
        res.json(result);
    }catch(err){
        next(err);
    }
}

module.exports.store = async (req, res, next) => {
    try{
        const result = await statusModel.store(req.body);
        res.json(result);
    }catch(err){
        next(err);
    }  
}