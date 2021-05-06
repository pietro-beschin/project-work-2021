const express = require('express');
const router = express.Router();
const historyController = require('./history/history.controller');
const statusController = require('./status/status.controller');

router.get('/history', historyController.list);
router.get('/status', statusController.getStatus);

module.exports = router;