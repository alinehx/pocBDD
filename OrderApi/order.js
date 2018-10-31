'use strict';

let fileSystem = require('fs');
const existingMovementPathFile = './mock/existingMovement.json';

let findExistingMovement = (callback) => {  
  fileSystem.readFile(existingMovementPathFile, 'utf8', function (err, data) {
    if (err) return callback('error');
    return callback(null, JSON.parse(data));
  });  
};

let cleanExistingMovement = (callback) => {
  fileSystem.truncate(existingMovementPathFile, 0, function(err, data) {
    if (err) return callback('error');
    return callback(null, 'ok');
  });
};

let createNewMovementFile = (object, callback) => {
  let objecStringify = JSON.stringify(object);
  fileSystem.writeFile(existingMovementPathFile, objecStringify, function(err) {
    if (err)  callback(err);
    return callback(null, objecStringify);
  });
};

let checkDifferenceNewMovementsAndExisting = (newMovement, existingMovement) => {
  if (!existingMovement) return null;
  return existingMovement.filter(function(obj) {
     return !newMovement.some(function(obj2) {
       return obj.value == obj2.value;
     });
   });
};

let changeExistingMovementToChargeBack = (existingMovement, objectDifference) => {
 existingMovement.forEach((element, index) => {
   if(objectDifference.indexOf(element) >= 0 ) {
     existingMovement[index].chargeBack = true;
   }
 });
 return existingMovement;
};

let newMovement = (req, res) => {  
  let newMovement = req.body.movements;
  
  findExistingMovement(function(err, order) {
    if (err) {
      res.status(500).send('error');
    }

    let existingMovement = order.movements;
    let objectDifference = checkDifferenceNewMovementsAndExisting(newMovement, existingMovement);

    if (objectDifference) {
      existingMovement = changeExistingMovementToChargeBack(existingMovement, objectDifference);
    }

    var orderMovement = {
      id: 1,
      movements: existingMovement.concat(newMovement)
    };
     
    res.status(201).send(orderMovement);
  });
  
};

let createMovement =  (req, res) => {
  let newMovement = req.body;
  cleanExistingMovement(function (err, fileEmpty) {
    if (err) res.status(500);

    createNewMovementFile(newMovement, function (err, data) {
      if (err) res.status(500);

      res.status(201).send(newMovement);
    });
    
  });
  
};

module.exports = {
  newMovement: newMovement,
  createMovement: createMovement
};