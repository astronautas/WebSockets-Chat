var Dispatcher = (function() {

	var routes = {
		"login"        : [],
		"add_contact"  : [],
		"chat"         : [],
		"send_message" : [],
		"contacts"     : [],
		"open_chat"    : [],
		'delete_contact' : [],
		'update_chat_title' : [],
		'load_top_authors' : []
	};

	var subscribe = function(event, method) {
		if (routes[event]) {
			routes[event].push(method);
		}
	};

	var dispatch = function(event, data) {
		var methods = routes[event];

		if (methods.length) {
			$.each(methods, function(index, method) {
				method(data);
			});
		}
	};

	return {
		dispatch  : dispatch,
		subscribe : subscribe
	} 
})();

module.exports = Dispatcher;