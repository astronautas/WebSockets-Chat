var ee = require('event-emitter');

var Session = (function() {
	var connection;
	var connectionEe = ee();

	var init = function(callback) {
		connection = new WebSocket("ws://127.0.0.1:5000/Chat?sessionKey=" + window.key);

		connection.onopen = function() {
			var test = connection;

			callback();
		}

		connection.onmessage = function(event) {
			var data = JSON.parse(event.data);
			var cmd  = data.Command;

			connectionEe.emit(cmd, data);
		};
	};

	var getConnection = function() {
		return connection;
	};

	return {
		init          : init,
		getConnection : getConnection,
		key           : "12345678912345678900",
		connectionEe  : connectionEe
	}
})();

module.exports = Session;