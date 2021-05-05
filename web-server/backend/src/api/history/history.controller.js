const historyModel = require('./history.model');

module.exports.list = async (req, res, next) => {
    const list = await historyModel.list();
    res.json(list);
}