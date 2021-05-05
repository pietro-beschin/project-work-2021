const moment = require('moment');
const fs = require('fs');
const file = '../../db.json';

module.exports.list = async () => {
    const products = require(file);
    return products;
}