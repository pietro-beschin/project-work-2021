const historyModel = require('./history.model');

module.exports.list = async (req, res, next) => {
    const list = await historyModel.list(req.query);
    res.json(list);
}

module.exports.store = async (req, res, next) => {
    const result = await historyModel.store(req.body);
    res.json(result);
}