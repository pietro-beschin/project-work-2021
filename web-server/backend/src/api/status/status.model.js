module.exports.getStatus = () => {
    mongoose.connect('mongodb://localhost:27017/project-work-2021/status', {useNewUrlParser: true, useUnifiedTopology: true});
    return "ciao";
}