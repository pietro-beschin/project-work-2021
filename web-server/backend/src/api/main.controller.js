const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');
const mergeJson = require('merge-json');

module.exports.getHistoryStatus = async (req, res, next) => {
    const result = {};

    result.status = await statusModel.getStatus();
    result.history = await historyModel.list({});

    res.json(result);
}