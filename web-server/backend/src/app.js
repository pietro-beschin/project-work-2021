const express = require('express');
const app = express();
const routes = require('./api/router');
const morgan = require('morgan');
const mongoose = require('mongoose');
const cors = require('cors');


mongoose.set('debug', true);        //per settare il debug globalmente
                                    //i connect sono inseriti nei controller così che
                                    //history e status si colleghino a collection diverse

app.use(morgan('tiny'));
app.use(cors());
//body parser
app.use(express.json({extended : true}));
app.use('/api', routes);

module.exports = app;