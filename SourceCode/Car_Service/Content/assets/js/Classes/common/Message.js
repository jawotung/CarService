(function (global, $) {
    const Message = function (msgType, msgTitle, msg, func, trxId) {
        return new Message.init(msgType, msgTitle, msg, func, trxId);
    }
    Message.init = function (msgType, msgTitle, msg, func, trxId) {
        this.msgType = msgType || "";
        this.msgTitle = msgTitle || "";
        this.msg = msg || "";
        this.trxId = trxId || "";
        this.func = func || "";

        this.msgToastrType = {
            "info": "fa fa-info-circle",
            "warning": "fas fa-lg fa-fw m-r-10 fa-exclamation-triangle",
            "success": "fa fa-check",
            "error": "fa fa-times-circle",
        };
        this.txtColor = {
            "info": "blue",
            "warning": "yellow",
            "success": "green",
            "error": "red",
        };
        this.JawoPogi = [
            {
                language: 'ENG',
                Msg:
                {
                    "info": "Message!",
                    "warning": "Warning!",
                    "success": "Success!",
                    "error": "Error!",
                },
                Confirmation:
                {
                    Yes: "Yes",
                    No: "No"
                }
            },
            {
                language: 'JP',
                Msg:
                {
                    "info": "メッセージ!",
                    "warning": "警告!",
                    "success": "成功!",
                    "error": "エラー!",
                },
                Confirmation:
                {
                    Yes: "はい",
                    No : "いいえ"
                }
            },
        ];
        this.messageTitle = this.JawoPogi.find(({ language }) => language == ($Helper().GetStoreData("SystemLanguage") == "JP" ? "JP" : "ENG"));
    }
    Message.prototype = {
        getError: function () {
            var errorList = "";
            if (Array.isArray(this.msg) || typeof this.msg === "object") {
                $.each(this.msg, function (key, value) {
                    errorList += '<div>' + (key + 1) + ". " + value + '</div>';
                });
            } else {
                errorList += '<div>' + this.msg + '</div>';
            }
            this.msg = errorList;
        },
        showToastrMsg: function () {
            if (this.msgType === "error") {
                this.getError();
            }
            iziToast.show({
                title: this.msgTitle,
                message: this.msg,
                icon: this.msgToastrType[this.msgType],
                position: 'topRight',
                backgroundColor: '',
                theme: 'light', // dark
                color: this.txtColor[this.msgType], // blue, red, green, yellow
                timeout: 5000,
            });
            return this;
        },
        showInfo: function (msg) {
            this.msgType = this.messageTitle.Msg[this.msgType];
            this.msgTitle = "Message!";
            this.msg = msg;
            this.showToastrMsg();
        },
        showError: function (msg) {
            this.msgType = "error";
            this.msgTitle = this.messageTitle.Msg[this.msgType];
            this.msg = msg;
            this.showToastrMsg();
        },
        showSuccess: function (msg) {
            this.msgType = "success";
            this.msgTitle = this.messageTitle.Msg[this.msgType];
            this.msg = msg;
            this.showToastrMsg();
        },
        showWarning: function (msg) {
            this.msgType = "warning";
            this.msgTitle = this.messageTitle.Msg[this.msgType];
            this.msg = msg;
            this.showToastrMsg();
        },
        showNoTimeout: function () {
            if (this.msgType === "error") {
                this.getError();
            }
            iziToast.show({
                title: this.msgTitle,
                message: this.msg,
                icon: this.msgToastrType[this.msgType],
                position: 'topRight',
                backgroundColor: '',
                theme: 'light', // dark
                color: this.txtColor[this.msgType], // blue, red, green, yellow
                progressBar: false,
                timeout: 0,
            });
            return this;
        },
        showErrorNoTimeout: function (msg) {
            this.msgType = "error";
            this.msgTitle = this.messageTitle.Msg[this.msgType];
            this.msg = msg;
            this.showNoTimeout();
        },
        showSuccessNoTimeout: function (msg) {
            this.msgType = "success";
            this.msgTitle = this.messageTitle[this.msgType];
            this.msg = msg;
            this.showNoTimeout();
        },
        confirmAction: function () {
            var self = this;
            var promiseObj = new Promise(function (resolve, reject) {
                iziToast.question({
                    timeout: 20000,
                    close: false,
                    overlay: true,
                    displayMode: 'once',
                    id: 'question',
                    zindex: 99999999,
                    title: self.msgTitle,
                    message: self.msg,
                    position: 'topCenter',
                    timeout: 0,
                    buttons: [
                        ['<button>' + self.messageTitle.Confirmation.Yes + '</button>', function (instance, toast) {
                            instance.hide({ transitinoOut: 'fadeOut' }, toast, 'button');
                            resolve(true);
                        }],
                        ['<button>' + self.messageTitle.Confirmation.No + '</button>', function (instance, toast) {
                            instance.hide({ transitinoOut: 'fadeOut' }, toast, 'button');
                        }, true],
                    ],
                });
            });
            return promiseObj;
        },
        confirmActionOkOnly: function () {
            var self = this;
            var promiseObj = new Promise(function (resolve, reject) {
                iziToast.question({
                    timeout: 20000,
                    close: false,
                    overlay: true,
                    displayMode: 'once',
                    id: 'question',
                    zindex: 99999999,
                    title: self.msgTitle,
                    message: self.msg,
                    position: 'topCenter',
                    timeout: 0,
                    buttons: [
                        ['<button>OK</button>', function (instance, toast) {
                            instance.hide({ transitinoOut: 'fadeOut' }, toast, 'button');
                            resolve(true);
                        }],
                    ],
                });
            });
            return promiseObj;
        },
        piConfirmationToNext: function () {
            var self = this;
            var promiseObj = new Promise(function (resolve, reject) {
                iziToast.destroy();
                iziToast.show({
                    timeout: 0,
                    close: false,
                    theme: 'dark',
                    icon: 'fas fa-lg fa-fw m-r-10 fa-exclamation-triangle',
                    title: 'Warning',
                    message: 'Your data is incomplete do you want to proceed?',
                    position: 'bottomCenter',
                    zindex: 99999999,
                    buttons: [
                        ['<button>Yes</button>', function (instance, toast) {
                            instance.hide({ transitinoOut: 'fadeOut' }, toast, 'button');
                            resolve(true);
                        }, true], // true to focus
                        ['<button>Close</button>', function (instance, toast) {
                            instance.hide({
                                transitionOut: 'fadeOutUp',
                                onClosing: function (instance, toast, closedBy) {
                                    console.info('closedBy: ' + closedBy); // The return will be: 'closedBy: buttonName'
                                }
                            }, toast, 'buttonName');
                        }]
                    ],
                });
            });
            return promiseObj;
        },
    }
    Message.init.prototype = Message.prototype;
    return global.Message = global.$M = Message;
}(window, $));