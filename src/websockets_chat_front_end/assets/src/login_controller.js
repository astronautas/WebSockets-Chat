var Dispatcher = require("./dispatcher.js");

var LoginController = (function() {

	var init = function() {
		Dispatcher.subscribe("login", login);
	};

	var login = function() {
		console.log("got loggin");
	};

	return {
		init: init
	}
})();

module.exports = LoginController;