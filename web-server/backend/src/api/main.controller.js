const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');
const mergeJson = require('merge-json');

module.exports.getHistoryStatus = async (req, res, next) => {
    const nullObj = "";
    res.json(await historyModel.list({}) + await statusModel.getStatus());
}