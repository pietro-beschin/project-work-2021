const statusModel = require('./status.model');
const mongoose = require('mongoose');

module.exports.getStatus = async (req, res, next) => {
    try{
        const status = await statusModel.getStatus();
        res.json(status);
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