var LoginPage = (function () {
    var tpl = require('../templates/login.html');

    var init = function (options) {
        render();

        // Events
        $('.js-login-submit').on('click', function() {
            Dispatcher.dispatch('login');
        });
    };

    var render = function () {
        $('.js-page-content').html(tpl());
    };

    return {
        init: init
    };
})();

module.exports = LoginPage;