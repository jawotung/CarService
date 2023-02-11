(function () {
    var notificationHub = $.connection.notification;
    notificationHub.client.addMessage = function (notify) {
        if (notify) {
            CUI.getNotifications();
        }
    };
    $.connection.hub.start()
        .done(function () {
            CUI.getNotifications();
        })
        .fail(function () {
            console.log("ERROR!!!")
        });

    notificationHub.client.sendNotification = function () { CUI.getNotifications(); }



})();