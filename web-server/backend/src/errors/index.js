const notFound = require('./not-found.handler');
const internalError = require('./internal.handler');
const validationError = require('./validation-error');

module.exports = [notFound, validationError, internalError];