const moment = require('moment');
const historySchema = require('./history.schema');
const history = require('./history.schema');

module.exports.list = async () => {
    return await historySchema.find();
}