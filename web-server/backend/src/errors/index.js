const notFound = require('./not-found.handler');
const internalError = require('./internal.handler');

module.exports = [notFound, internalError];