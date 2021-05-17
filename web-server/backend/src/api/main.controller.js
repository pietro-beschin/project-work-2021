const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');

module.exports.getHistoryStatus = async (res, next, res) => {
    res.json(await historyModel.list() + await statusModel.getStatus());
}