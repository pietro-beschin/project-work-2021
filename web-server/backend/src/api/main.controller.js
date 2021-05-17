const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');
const mergeJson = require('merge-json');

module.exports.getHistoryStatus = async (req, res, next) => {
    const nullObj = {};
    res.json(mergeJson.merge(await historyModel.list(nullObj), await statusModel.getStatus()));
}