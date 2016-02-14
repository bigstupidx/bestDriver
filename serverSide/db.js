var mongodb= require('mongodb')
var config = require('./config.js');
var c = new config();

function insertRecord(object,res){
	var MongoClient = mongodb.MongoClient;
	MongoClient.connect(c.getMongoConnection(), function(err, db) {
		if (err) {
			console.dir(err);
			res.json("Not able to connect to database");
		}
		var collection = db.collection('ranking');
		var options = {
			"limit": 1,
			"sort": [['_id','-1']]
		};
		collection.find({},options).toArray(function(err, docs) {
			if(docs[0]){
				object["idranking"] =docs[0].idranking+1;
			}else{
				object["idranking"] =1
			}
			collection.insert(object, {w:1},function(err, result) {
				if (err) {
					console.dir(err);
					res.json(false);
					db.close();
				} else {
					res.json(true);
					db.close();
				}
			});
		});
	});
}

function getRecords(limitValue,res){
	var MongoClient = mongodb.MongoClient;
	MongoClient.connect(c.getMongoConnection(), function(err, db) {
		if (err) {
			console.dir(err);
			res.json("Not able to connect to database");
		}
		var collection = db.collection('ranking');

		collection.find({},limitValue).toArray(function(err, docs) {
			if (err) {
				console.log("Error");
				console.dir(err);
				res.json(false);
				db.close();
			} else {
				res.json(docs);
				db.close();
			}
		});
		
	});
}


function deleteRecord(id, res){
	var MongoClient = mongodb.MongoClient;
	MongoClient.connect(c.getMongoConnection(), function(err, db) {
		if (err) {
			console.dir(err);
			res.json("Not able to connect to database");
		}
		var collection = db.collection('ranking');
		console.log(id);
		collection.remove(id, {w:1} ,function(err, doc) {
			if (err) {
				console.log("Erro");
				console.dir(err);
				res.json(false);
				db.close();
			} else {
				res.json(true);
				db.close();
			}
		});
		
	});
}

exports.insertRecord = insertRecord;
exports.getRecords = getRecords;
exports.deleteRecord = deleteRecord;
