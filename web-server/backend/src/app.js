const express = require('express');
const app = express();
const routes = require('./api/router');
const morgan = require('morgan');
const mongoose = require('mongoose');
const cors = require('cors');

//mongoose.connect('mongodb://localhost:27017/project-work-2021', {useNewUrlParser: true, useUnifiedTopology: true}); //password : i2XjAmcFr8NM
//10.0.25.201
mongoose.connect('mongodb://root:i2XjAmcFr8NM@10.0.25.201:27017/project-work-2021', {useNewUrlParser: true});
mongoose.set('debug', true);

app.use(morgan('tiny'));
app.use(cors());
//body parser
app.use(express.json({extended : true}));
app.use('/api', routes);

module.exports = app;