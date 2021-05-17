const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');

module.exports.getHistoryStatus = async (req, res, next) => {
    res.json(await historyModel.list({}) + await statusModel.getStatus());
}