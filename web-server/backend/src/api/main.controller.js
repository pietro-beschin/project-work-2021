const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');

module.exports.getHistoryStatus = async (req, res, next) => {
    const result = {};

    result.status = await statusModel.getStatus();
    result.history = await historyModel.getLastCommessa();

    res.json(result);
}