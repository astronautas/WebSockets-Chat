require("!style-loader!css-loader!sass-loader!./../sass/top_authors.scss");
var Dispatcher = require("./../dispatcher.js");

var TopAuthorsPage = function(options) {
	var tpl = require("./../templates/top_authors.html");

	// Request data contains all room data
	// Extract the
	var render = function(requestData) {
		var rendered = tpl(requestData);
		parent.html(rendered);
	};

	// Constructor
	var parent   = options.parent;
	var emitter  = options.emitter;

	emitter.on('update', render);

	Dispatcher.dispatch('load_top_authors', {});
};

module.exports = TopAuthorsPage;