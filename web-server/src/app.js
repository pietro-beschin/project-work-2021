const express = require('express');
const app = express();

app.use(morgan('tiny'));
//body parser
app.use(express.json({extended : true}));


module.exports = app;