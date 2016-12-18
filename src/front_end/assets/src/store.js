var Session    = require("./session.js");
var ee         = require('event-emitter');
var Dispatcher = require("./dispatcher.js");

var Store = (function() {
	// Data
	var contacts = {};
	var chatRoomData = {};
	var topAuthorsData = {};

	var contactsEe = ee();
	var chatRoomEe = ee();
	var topAuthorsEe = ee();

	var init = function() {

		// Subscribe to dispatcher actions
		Dispatcher.subscribe("add_contact",       addContact);
		Dispatcher.subscribe('delete_contact',    deleteContact);
		Dispatcher.subscribe("send_message",      sendMessage);
		Dispatcher.subscribe('contacts',          getContacts);
		Dispatcher.subscribe('open_chat',         getChatRoomData);
		Dispatcher.subscribe('update_chat_title', updateChat);
		Dispatcher.subscribe('load_top_authors',  load_top_authors);

		// Subscribe to websocket session events
		Session.connectionEe.on('contacts_data', updateContacts);
		Session.connectionEe.on('chat_room_data', updateChatRoomData);
		Session.connectionEe.on('top_authors', updateTopAuthors);
	};

	// Top authors
	var load_top_authors = function() {
		requestData('top_authors', {});
	}

	var updateTopAuthors = function(data) {
		topAuthorsData = data;
		topAuthorsEe.emit('update', data);
	};

	var addContact = function(data) {
		requestData("add_contact", data);
	};

	var deleteContact = function(data) {
		requestData('delete_contact', data);
	};

	var getContacts = function(callback) {
		requestData("contacts", {});
	};

	var updateContacts = function(data) {
		contacts = data;
		contactsEe.emit('update', contacts);
	};

	// Chat rooms
	var getChatRoomData = function(callback) {
		requestData("chat", callback);
	};

	var updateChatRoomData = function(data) {
		var chatRoomId = data.data.chat_id;
		var chat;

		chatRoomData = data;
		chatRoomEe.emit('update', data);
	};

	var updateChat = function(data) {
		requestData('update_chat', data);
	};

	var sendMessage = function(data) {
		requestData('send_message', data);
	};

	var requestData = function(command, data) {

		var requestData = {
			SessionKey : window.key,
			Command    : command,
			data       : data
		};

		Session.getConnection().send(JSON.stringify(requestData));
	};

	var currChatRoomData = function() {
		return chatRoomData;
	};

	return {
		init            : init,
		contactsEe      : contactsEe,
		chatRoomEe      : chatRoomEe,
		topAuthorsEe    : topAuthorsEe,
		getContacts     : getContacts,
		currChatRoomData : currChatRoomData
	}
})();


module.exports = Store;