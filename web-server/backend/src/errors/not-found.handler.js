module.exports = (err, req, res, next) => {
    if (err.message === 'Not Found') {
        res.status(404);
        res.send('ID non esistente');
    }
    else if(err.message === 'Empty DB'){
        res.status(404);
        res.send('Impossibile trovare ultima commessa in un database vuoto');
    }else{
        next(err);
    }
}