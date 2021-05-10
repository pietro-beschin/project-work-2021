const status = require('./status.schema');

module.exports.getStatus = async () => {
    return await status.findOne();
}