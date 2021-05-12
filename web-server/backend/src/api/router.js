const express = require('express');
const router = express.Router();
const historyController = require('./history/history.controller');
const statusController = require('./status/status.controller');

router.get('/history', historyController.list);
router.get('/lastComessa', historyController.getLastCommessa);
router.get('/status', statusController.getStatus);
router.post('/history', historyController.store);
router.post('/status', statusController.store);

module.exports = router;