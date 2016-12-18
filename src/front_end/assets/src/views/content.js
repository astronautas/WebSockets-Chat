var Store      = require("./../store.js");
var Dispatcher = require("./../dispatcher.js");

var ContactsPage = require("./contacts_page.js");
var ChatRoomPage = require('./chat_room_page.js');
var TopAuthorsPage = require('./top_authors_page.js');

var Content = (function() {
	var $el;

	var init = function() {
		$el = $('.js-page-content');

		ContactsPage.init({ parent: $el, emitter: Store.contactsEe, store: Store.contacts });

		// Initial load
		Dispatcher.dispatch('contacts', {});

		// Page change events
		ContactsPage.contactsPageEe.on('open_chat', function(data) {
			$el.html('');

			var el = new ChatRoomPage({ parent: $el, emitter: Store.chatRoomEe, store: Store.currChatRoomData });
		});

		ContactsPage.contactsPageEe.on('open_top_authors', function() {
			$el.html('');

			new TopAuthorsPage({ parent: $el, emitter: Store.topAuthorsEe })
		});
	};

	return {
		init: init
	}
})();

module.exports = Content;