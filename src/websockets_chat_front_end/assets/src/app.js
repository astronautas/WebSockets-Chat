require("!style-loader!css-loader!sass-loader!./sass/header.scss");

var Dispatcher      = require("./dispatcher.js");
var Session         = require("./session.js");
var LoginController = require("./login_controller");
var LoginPage       = require("./views/login_page.js");
var Store           = require("./store.js");

var Content = require('./views/content.js');

module.exports = (function () {

    var key = prompt("Session key");
    window.key = key;

    $(document).ready(function () {
        //LoginPage.init({ tpl: loginTpl });
        //LoginController.init();
        Session.init(function() {
            Store.init();
            Content.init();
        });

    });
})();