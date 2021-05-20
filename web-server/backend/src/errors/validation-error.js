module.exports = (err, req, res, next) => {
    if (err.name === 'ValidationError') {
        res.status(400);
        res.send(err.message);
    } else {
        next(err);
    }
}