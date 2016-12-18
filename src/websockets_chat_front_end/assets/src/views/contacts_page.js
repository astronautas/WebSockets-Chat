require("!style-loader!css-loader!sass-loader!./../sass/contacts.scss");
var ee         = require('event-emitter');

var Store      = require("../store.js");
var Dispatcher = require("./../dispatcher.js");

var ContactsPage = (function() {
	var tpl = require("./../templates/contacts.html");
	var parent;
	var emitter;
	var store;

	var contactsPageEe = ee();

	var init = function(options) {
		parent  = options.parent;
		emitter = options.emitter;
		store   = options.store;

		loadEvents();

		emitter.on('update', render);

		// Initial load
		if (store) render(store);
	};

	var render = function(contacts) {
		var rendered = tpl(contacts);

		parent.html(rendered);
	};

	var loadEvents = function() {
		$(document).on('submit', '.js-add-contact-form', addContact);
		$(document).on('click', '.js-delete-contact', removeContact);
		$(document).on('click', '.js-contact', openChat);
		$(document).on('click', '.js-open-top-authors', openTopAuthors);
	};

	var openTopAuthors = function() {
		contactsPageEe.emit('open_top_authors', {});
	};

	var openChat = function(e) {
		e.preventDefault();

		var $currContact = $(e.currentTarget).closest('tr');
		var email  = $currContact.attr('data-email');
		var chatId = $currContact.attr('data-chat-id');

		if (!email) return;

		//Dispatcher.dispatch("chat", { email: email, chatId: chatId });
		Dispatcher.dispatch('open_chat', { email: email, chatId: chatId })
		contactsPageEe.emit('open_chat', { email: email, chatId: chatId });
	}

	var addContact = function(e) {
		e.preventDefault();

		var $form = $(e.currentTarget);
		var email = $form.find('input[name="contact_email"]').val();

		if (!email) return;

		Dispatcher.dispatch("add_contact", { email: email })
	};

	var removeContact = function(e) {
		e.preventDefault();

		var $currContact = $(e.currentTarget).closest('tr');
		var email  = $currContact.attr('data-email');

		if (!email) return;

		Dispatcher.dispatch('delete_contact', { email: email })
	}

	return {
		init           : init,
		contactsPageEe : contactsPageEe
	}
})();

module.exports = ContactsPage;