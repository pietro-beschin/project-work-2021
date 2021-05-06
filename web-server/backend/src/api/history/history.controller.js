const historyModel = require('./history.model');
const mongoose = require('mongoose');

mongoose.connect('mongodb://localhost:27017/project-work-2021/history', {useNewUrlParser: true, useUnifiedTopology: true});


module.exports.list = async (req, res, next) => {
    const list = await historyModel.list();
    res.json(list);
}