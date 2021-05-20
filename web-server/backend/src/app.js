const express = require('express');
const app = express();
const routes = require('./api/router');
const morgan = require('morgan');
const mongoose = require('mongoose');
const cors = require('cors');
const errorHandlers = require('./errors');
const https = require('https');
const fs = require('fs');

var options = {
    key : fs.readFileSync('/opt/bitnami/apache2/conf/privkey.pem'),
    cert : fs.readFileSync('/opt/bitnami/apache2/conf/key-cert.pem'),
    passphrase : 'gruppo1gang'
};
mongoose.connect("mongodb://10.0.25.202:27017/project-work-2021", {useNewUrlParser: true, useUnifiedTopology: true});
mongoose.set('debug', true);
mongoose.set('useFindAndModify', false);

app.use(morgan('tiny'));
app.use(cors());
app.use(express.json({extended : true}));
app.use('/api', routes);
app.use(errorHandlers);

https.createServer(options, app).listen(8080);

module.exports = app;