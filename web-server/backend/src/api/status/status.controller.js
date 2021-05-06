const statusModel = require('./status.model');
const mongoose = require('mongoose');

module.exports.getStatus = async (req, res, next) => {
    const status = await statusModel.getStatus();
    res.json(status);
}