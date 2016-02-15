var express = require('express');
var crypto = require('crypto');
var request = require('request');
var bestDrive = express();
var mydb = require('./db.js');
bestDrive.use(express.bodyParser());
var conn;
var ranking;
var db;
var ipaddress = process.env.OPENSHIFT_NODEJS_IP || "127.0.0.1";
var port = process.env.OPENSHIFT_NODEJS_PORT || 3000,
    http = require('http'),
    fs = require('fs'),
    html = fs.readFileSync('index.html');
var config = require('./config.js');
var c = new config();

var log = function(entry) {
    fs.appendFileSync('bestDriver.log', new Date().toISOString() + ' - ' + entry + '\n');
};




bestDrive.get('/users', function(req, res) {
	var options = {
		"sort": [['score','desc']]
	};
	mydb.getRecords(options,res);
	
});

bestDrive.get('/users/limit/:limit', function(req, res) {
		var options = {
			"limit": req.params.limit,
			"sort": [['score','desc']]
		};
	mydb.getRecords(options,res);
});

bestDrive.delete('/user/delete/:id', function(req, res) {
	if(crypto.createHash('md5').update(req.body.name+req.body.score+c.getKey()).digest("hex") == hashValue){
		var options = {
			"idranking": parseInt(req.params.id)
		};
		mydb.deleteRecord(options, res);
	}
});

bestDrive.delete('/users/delete/all', function(req, res) {
	var hashValue = req.headers["hashvalue"];
	if(crypto.createHash('md5').update(req.body.name+req.body.score+c.getKey()).digest("hex") == hashValue){
		var options = {};
		mydb.deleteRecord(options, res);
	}
});

bestDrive.post('/user', function(req, res) {
	var post  = {};
	if(!req.body.hasOwnProperty('name')) {
		res.statusCode = 400;
		log("Name not found");
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
	var ipAddr = req.headers["x-forwarded-for"];
	var hashValue = req.headers["hashvalue"];
	if(crypto.createHash('md5').update(req.body.name+req.body.score+c.getKey()).digest("hex") == hashValue){
		request('http://freegeoip.net/json/' + ipAddr, function (error, response, body) {
			if (!error && response.statusCode == 200) {
					var obj = JSON.parse(body);
					post["country_code"] = obj.country_code;
					post["region_code"] = obj.region_code;
					post["city"] = obj.city;
					mydb.insertRecord(post, res);
			}else{
				mydb.insertRecord(post, res);
			}
		});
	}
	

	
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

bestDrive.listen(port,ipaddress);


