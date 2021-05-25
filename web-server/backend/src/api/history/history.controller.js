const historyModel = require('./history.model');
const mongoose = require('mongoose');

module.exports.list = async (req, res, next) => {       //restituisce le commesse secondo i filtri
    try{
        const list = await historyModel.list(req.query);
        res.json(list);
    }catch(err){
        next(err);
    }
}

let attesa_commesse = [];

module.exports.store = async (req, res, next) => {
    try{
        const result = await historyModel.store(req.body);   //prova a scrivere il primo elemento della coda
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
        await historyModel.delete(req.params.commessa);
        console.log("COMMESSA DA ELIMINARE : "+ req.params.commessa);
        res.json("RECORD CON CODICE_COMMESSA " + req.params.commessa + "ELIMINATO!");
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