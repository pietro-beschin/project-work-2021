const express = require('router');
const router = express.Router();
const historyController = require('./history/history.controller');
const statusController = require('./status/history.controller');

router.use('/history', historyRouter);
router.use('/status', statusRouter);


module.exports = router;