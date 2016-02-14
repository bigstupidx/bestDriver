var express = require('express');
var bestDrive = express();
var config = require('./config.js');
var c = new config();
bestDrive.use(express.bodyParser());
var conn;
var ipaddress = process.env.OPENSHIFT_NODEJS_IP || "127.0.0.1";
var port = process.env.OPENSHIFT_NODEJS_PORT || 3000,
    http = require('http'),
    fs = require('fs'),
    html = fs.readFileSync('index.html');

var log = function(entry) {
    fs.appendFileSync('/tmp/sample-app.log', new Date().toISOString() + ' - ' + entry + '\n');
};

function connectMySQL(){
	var mysql      = require('mysql');
	conn = mysql.createConnection({
		host     : c.getHost(),
		user     : c.getUser(),
		password : c.getPassword(),
		database : c.getDatabase(),
		connectionLimit: 15,
        queueLimit: 30,
        acquireTimeout: 1000000
	});
	conn.connect();
}

function disconnectMySQL(){
	conn.end();
	log("disconnected");
}

bestDrive.get('/users', function(req, res) {
  connectMySQL();
  var response = [];
  conn.query('SELECT name, IFNULL(ip,"") as ip, score FROM ranking order by score desc', function(err, rows, fields) {
		if (!err){
			res.send(rows);
		} else {
			log('Error while performing Select Query.');
			log(err);
		}

		
	});
	disconnectMySQL();
	
});

bestDrive.get('/users/limit/:limit', function(req, res) {
  connectMySQL();
  var response = [];
  conn.query('SELECT name, IFNULL(ip,"") as ip, score FROM ranking order by score desc limit '+req.params.limit, function(err, rows, fields) {
		if (!err){
			res.send(rows);
		} else {
			log('Error while performing Select Limit Query.');
			log(err);
		}

		
		});
		disconnectMySQL();
	
});

bestDrive.delete('/user/delete/:id', function(req, res) {
   connectMySQL();
  var response = [];
  conn.query('DELETE FROM ranking WHERE idranking = '+req.params.id, function(err, rows, fields) {
		if (!err){
			res.json(true);
		} else {
			log("Error to delete");
			res.json(false);
		}
		});
		disconnectMySQL();
});

bestDrive.delete('/users/delete/all', function(req, res) {
   connectMySQL();
  var response = [];
  conn.query('DELETE FROM ranking', function(err, rows, fields) {
		if (!err){
			res.json(true);
		} else {
			log("Error to delete");
			res.json(false);
		}
		});
		disconnectMySQL();
});

bestDrive.post('/user', function(req, res) {
	connectMySQL();
	var post  = {};
	if(!req.body.hasOwnProperty('name')) {
		res.statusCode = 400;
		log("NAme not found");
		return res.send('Error 400: Post syntax incorrect.');
	} else{
		post["name"] = req.body.name;
	}
	if(!req.body.hasOwnProperty('score')) {
		res.statusCode = 400;
		log("Score not found");
		return res.send('Error 400: Post syntax incorrect.');
	} else{
		post["score"] = req.body.score;
	}
	if(req.body.hasOwnProperty('ip')) {
		post["ip"] = req.body.ip;
	}
	var query = conn.query('INSERT INTO ranking SET ?', post, function(err, result) {
		if (!err){
			log('The solution is: ', result);
		} else{
			log('Error while performing Insert Query.');
			log(err);
		}
	});
	log(query.sql);
	disconnectMySQL();
	res.json(true);
});

bestDrive.use(function logJsonParseError(err, req, res, next) {
  if (err.status === 400 && err.name === 'SyntaxError' && err.body) {
    // Display extra information for JSON parses
    log('JSON body parser error!')
    log(req.method + ' ' + req.url)
    log(err.body.slice(0, 100).toString())
  }

  next(err)
})

bestDrive.listen(port);


