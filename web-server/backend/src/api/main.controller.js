const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');
const mergeJson = require('merge-json');

module.exports.getHistoryStatus = async (req, res, next) => {
    res.json(mergeJson.merge(await historyModel.list({}), await statusModel.getStatus()));
}