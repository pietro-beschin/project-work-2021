const historyModel = require('./history/history.model');
const statusModel = require('./status/status.model');

module.exports.getLastCommessaStatus = async (req, res, next) => {
    let result = {};

    result.status = await statusModel.getStatus();
    result.history = await historyModel.getLastCommessa();

    res.json(result);
}

module.exports.getHistoryStatus = async (req, res, next) => {
    let result = {};
    result.status = await statusModel.getStatus();
    result.history = await historyModel.list({});

    res.json(result);
}
