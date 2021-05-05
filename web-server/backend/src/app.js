const express = require('express');
const app = express();
const routes = require('./api/router');
const morgan = require('morgan');

app.use(morgan('tiny'));
//body parser
app.use(express.json({extended : true}));
app.use('/api', routes);

module.exports = app;