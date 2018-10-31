'use strict';

const order = require('../order');

module.exports = function routes(app) {
  app.post('/api/orders/movement/:id', function(req, res) {    
    return order.newMovement(req, res);
  });

  app.post('/api/orders/movement', function (req, res) { 
    return order.createMovement(req, res);
  });
};