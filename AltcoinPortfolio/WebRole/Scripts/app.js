'use strict';

let vm = new Vue({
    router,
    el: '#portoflio-app',
    data () {
        return {
            title: 'nbu-altcoin-portfolio'
        }
    }
});

$(function () {
    var notificationHub = $.connection.notificationHub;

    notificationHub.client.reload = function () {
        debugger;
        location.reload();
    };

    $.connection.hub.start();
});