const express = require('express');
const app = express();
const routes = require('./api/router');
const morgan = require('morgan');
const mongoose = require('mongoose');
const cors = require('cors');

mongoose.connect("mongodb://10.0.25.202:27017/project-work-2021", {useNewUrlParser: true, useUnifiedTopology: true});
mongoose.set('debug', true);
mongoose.set('useFindAndModify', false);

app.use(morgan('tiny'));
app.use(cors());
app.use(express.json({extended : true}));
app.use('/api', routes);

module.exports = app;