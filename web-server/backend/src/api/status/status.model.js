const statusSchema = require('./status.schema');

module.exports.getStatus = async () => {
    return await statusSchema.findOne();
}

module.exports.store = async (data) => {
    if(await statusSchema.findOne()){
        return await statusSchema.findOneAndUpdate(data);
    }
    return await statusSchema.create(data);
}