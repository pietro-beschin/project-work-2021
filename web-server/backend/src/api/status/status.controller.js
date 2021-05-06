const statusModel = require('./status.model');
const mongoose = require('mongoose');

mongoose.connect('mongodb://localhost:27017/project-work-2021/status', {useNewUrlParser: true, useUnifiedTopology: true});

module.exports.getStatus = async (req, res, next) => {
    const status = await statusModel.getStatus();
    res.json(status);
}