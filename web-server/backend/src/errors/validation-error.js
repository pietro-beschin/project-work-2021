module.exports = (err, req, res, next) => {
    if (err.name === 'ValidationError') {       //se il formato del json e del database non coincidono
        res.status(400);
        res.send(err.message);
    } else {
        next(err);
    }
}