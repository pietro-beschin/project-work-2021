const moment = require('moment');
const fs = require('fs');
const file = require('../../db.json');

module.exports.list = async (query) => {
    const products = await fs.readFile(file);
    return products;
}