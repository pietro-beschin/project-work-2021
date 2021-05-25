const express = require('express');
const app = express();
const routes = require('./api/router');
const morgan = require('morgan');
const mongoose = require('mongoose');
const cors = require('cors');
const errorHandlers = require('./errors');
const historyController = require('./api/history/history.controller');

//connessione a mongodb
mongoose.connect("mongodb://10.0.25.202:27017/project-work-2021", {useNewUrlParser: true, useUnifiedTopology: true});

mongoose.connection.on('open', function (ref) {
    console.log('Connected to mongo server.');
    historyController.store();
});

mongoose.connection.on('disconnected', function(ref) {
    console.log('Not connected to mongo server.');
});

mongoose.set('debug', true);
mongoose.set('useFindAndModify', false);

app.use(morgan('tiny'));
app.use(cors());
app.use(express.json({extended : true}));
app.use('/api', routes);
app.use(errorHandlers);

module.exports = app;