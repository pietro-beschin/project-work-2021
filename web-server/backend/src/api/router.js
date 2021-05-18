const express = require('express');
const router = express.Router();
const historyController = require('./history/history.controller');
const statusController = require('./status/status.controller');
const mainController = require('./main.controller');

router.get('/history', historyController.list);
router.get('/lastCommessa', historyController.getLastCommessa);
router.get('/status', statusController.getStatus);
router.post('/history', historyController.store);
router.post('/status', statusController.store);
router.delete('/history/:id', historyController.delete);
router.delete('/history', historyController.clear);
router.get('/historyStatus', mainController.getHistoryStatus);
router.get('/lastCommessaStatus', mainController.getLastCommessaStatus);


module.exports = router;