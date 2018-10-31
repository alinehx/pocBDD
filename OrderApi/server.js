'use strict';

let express = require('express');
let app = express();
let port = process.env.PORT || 3500;

// json encoded bodies
var bodyParser = require('body-parser');
app.use(bodyParser.json()); 
app.use(bodyParser.urlencoded({ extended: true }));

require('./routes/order')(app);

// start the server
app.listen(port);
console.log('Server started! At http://localhost:' + port);