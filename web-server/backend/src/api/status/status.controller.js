const statusModel = require('./status.model');

module.exports.getStatus = async (req, res, next) => {
    const status = await statusModel.getStatus();
    res.json(status);
}