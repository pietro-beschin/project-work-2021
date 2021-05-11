const status = require('./status.schema');

module.exports.getStatus = async () => {
    return await status.findOne();
}

module.exports.store = async (data) => {
    return await status.findOneAndUpdate(data);
}