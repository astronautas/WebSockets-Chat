require("!style-loader!css-loader!sass-loader!./../sass/chat.scss");
var Dispatcher = require("./../dispatcher.js");

var ChatRoomPage = function(options) {
	var tpl = require("./../templates/chat_room.html");
	var title = "";

	// Request data contains all room data
	// Extract the
	var render = function(requestData) {
		var rendered = tpl(requestData.data);
		parent.html(rendered);
	};

	var sendMessage = function(e) {
		e.preventDefault();

		var $form = $(e.currentTarget);

		var msg    = $form.find('input[name="message"]').val();
		var chatId = $form.attr('data-chat-id'); 

		Dispatcher.dispatch("send_message", { msg: msg, chatId: chatId });
	};

	var updateTitle = function(e) {
		var $field = $(e.currentTarget);
		var $form = $field.closest('.chat');

		var chatId = $form.attr('data-chat-id');

		if ($field.val() !== title) {
			title = $field.val();

			Dispatcher.dispatch("update_chat_title", { chat_room_name: title, chatId: chatId})
		}
	};

	var goBack = function() {
		contactsPageEe.emit('open_contacts', {});
	};

	var loadEvents = function() {
		$(document).on('submit', '.js-send-message-form', sendMessage);
		$(document).on('focusout', '.js-chat-name', updateTitle);
		$(document).on('click', '.js-back', goBack);
	};

	// Constructor
	var parent   = options.parent;
	var emitter  = options.emitter;
	var store    = options.store();

	emitter.on('update', render);

	if (store) {
		render(store);
	}

	loadEvents();
};

module.exports = ChatRoomPage;