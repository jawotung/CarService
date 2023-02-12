; (function () {
    const DataClass = function (formData, formAction) {
        return new DataClass.init(formData, formAction);
    }
    DataClass.init = function (formData, formAction) {
        $M.init.call(this);
        this.formData = formData || "";
        this.formAction = formAction || "";
        this.jsonData = {};
        this.submitType = "POST";
        this.responseData = {};
        this.LanguageData = {};
    }
    DataClass.prototype = {
        setJsonData: function () {
            let self = this;
            this.jsonData = {};
            if (self.formData) {
                $.each(self.formData, function () {
                    $(this).prop("disabled", "false");
                    if (self.jsonData[this.name]) {
                        if (!self.jsonData[this.name].push) {
                            self.jsonData[this.name] = [self.jsonData[this.name]];
                        }
                        self.jsonData[this.name].push(this.value.trim() || '');
                    } else {
                        self.jsonData[this.name] = this.value.trim() || '';
                    }
                });
            } else {
                self.showError("Please try again.");
            }
            return this;
        },
        setAndGetJsonData: function () {
            let self = this;
            let jsonData = {};

            $.each(formData, function () {
                $(this).prop("disabled", "false");
                if (jsonData[this.name]) {
                    if (!jsonData[this.name].push) {
                        jsonData[this.name] = [jsonData[this.name]];
                    }
                    jsonData[this.name].push(this.value.trim() || '');
                } else {
                    jsonData[this.name] = this.value.trim() || '';
                }
            });

            return jsonData;
        },
        clearFromData: function (formID) {
            let self = this;
            if (!formID) {
                self.showError("Please try again.");
                return;
            }
            $.each($("#" + formID + " .input"), function () {
                console.log(this);
                //$(this).prop("readonly", false);
                if ($(this).hasClass("input")) {
                    var type = $(this).attr('type');
                    var showIfTrue = "";
                    if (type === "text" || type === "hidden" || $(this).is("select") || $(this).is("textarea") || type == "number") {
                        $(this).val("");
                        if ($(this).hasClass('select2-hidden-accessible')) {
                            $(this).trigger('change');
                            $(this).val(null).trigger('change');
                        }
                    } else if (type === "file") {
                        $(this).val(null);
                    }
                    if (type === "radio") {
                        if ($(this).data("radiodefault")) {
                            $(this).trigger('click');
                        }
                        if ($(this).data("showiftrue")) {
                            if ($(this).is(":checked")) {
                                $(this).trigger('click');
                            }
                        }
                    }
                }
            });
            $("#" + formID).parsley().reset();
            return this;
        },
        partialClearFromData: function (formID) {
            let self = this;
            if (!formID) {
                self.showError("Please try again.");
                return;
            }
            $.each($("#" + formID + " .input"), function () {
                if ($(this).hasClass("input")) {
                    if (!$(this).hasClass("xPartialClear")) {
                        var type = $(this).attr('type');
                        var showIfTrue = "";
                        if (type === "text" || type === "hidden" || $(this).is("select") || $(this).is("textarea") || type == "number") {
                            $(this).val("");
                            if ($(this).hasClass('select2-hidden-accessible')) {
                                $(this).trigger('change');
                                $(this).val(null).trigger('change');
                            }
                        }
                        if (type === "radio") {
                            $(this).trigger('click');
                            $(this).prop("checked", false);
                            if ($(this).data("radiodefault")) {
                                $(this).trigger('click');
                            }
                            if ($(this).data("showiftrue")) {
                                if ($(this).is(":checked")) {
                                    $(this).trigger('click');
                                }
                            }
                        }
                    }
                }
            });
            $("#" + formID).parsley().reset();
            return this;
        },
        sendData: function () {
            let self = this;

            let promiseObj = new Promise(function (resolve, reject) {
                if (!self.formAction) {
                    self.showError("Please try again.");
                    resolve(false);
                    return;
                }
                $.ajax({
                    dataType: 'json',
                    type: self.submitType,
                    url: self.formAction,
                    data: self.jsonData,
                    beforeSend: function () {
                        $('#loading_modal').modal("show");
                    },
                    success: function (response) {
                        $('#loading_modal').modal('hide');
                        if (response.success) {
                            self.responseData = response.data;
                            self.msgType = "success";
                            self.msgTitle = self.messageTitle.Msg[self.msgType];
                            self.msg = response.msg;
                            resolve(true);
                        } else {
                            self.msgType = "error";
                            self.msgTitle = self.messageTitle.Msg[self.msgType];
                            if (response.hasOwnProperty('msg'))
                            {
                                self.msg = response.msg;
                            } else {
                                self.msg = response.errors;
                            }
                        }
                        if (response.hasOwnProperty('errors') || response.hasOwnProperty('msg')) {
                            if (response.hasOwnProperty('type')) {
                                if (response.type == "Login") {
                                    self.showSessionError(response);
                                }
                            } else {
                                self.showToastrMsg();
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $('#loading_modal').modal('hide');
                        console.table(jqXHR.status);
                        console.table(textStatus);
                        console.table(errorThrown);
                    }
                });
            });
            return promiseObj;
        },
        sendDataWithoutMsg: function () {
            let self = this;

            let promiseObj = new Promise(function (resolve, reject) {
                if (!self.formAction) {
                    self.showError("Please try again.");
                    resolve(false);
                    return;
                }
                $.ajax({
                    dataType: 'json',
                    type: self.submitType,
                    url: self.formAction,
                    data: self.jsonData,
                    beforeSend: function () {
                        $('#loading_modal').modal("show");
                    },
                    success: function (response) {
                        $('#loading_modal').modal('hide');
                        if (response.success) {
                            self.responseData = response.data;
                            self.msgType = "success";
                            self.msgTitle = self.messageTitle.Msg[self.msgType];
                            self.msg = response.msg;
                            resolve(true);
                        } else {
                            self.msgType = "error";
                            self.msgTitle = self.messageTitle.Msg[self.msgType];
                            if (response.hasOwnProperty('msg')) {
                                self.msg = response.msg;
                            } else {
                                self.msg = response.errors;
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $('#loading_modal').modal('hide');
                        console.table(jqXHR.status);
                        console.table(textStatus);
                        console.table(errorThrown);
                    }
                });
            });
            return promiseObj;
        },
        sendDataNoLoading: function () {
            let self = this;

            let promiseObj = new Promise(function (resolve, reject) {
                if (!self.formAction) {
                    self.showError("Please try again.");
                    resolve(false);
                    return;
                }
                $.ajax({
                    dataType: 'json',
                    type: self.submitType,
                    url: self.formAction,
                    data: self.jsonData,
                    beforeSend: function () {
                    },
                    success: function (response) {
                        if (response.success) {
                            self.responseData = response.data;
                            self.msgType = "success";
                            self.msgTitle = "Success!";
                            self.msg = response.msg;
                            resolve(true);
                        } else {
                            self.msgType = "error";
                            self.msgTitle = "Error!";
                            self.msg = response.errors || response.msg;;
                        }
                        if (response.hasOwnProperty('errors') || response.hasOwnProperty('msg')) {
                            if (response.hasOwnProperty('type')) {
                                if (response.type == "Login") {
                                    self.showSessionError(response);
                                }
                            } else {
                                self.showToastrMsg();
                            }

                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        //$('#loading_modal').modal('hide');
                        self.showError("An error occured while processing your request. Please refresh the page and try again.")
                        console.table(jqXHR.status);
                        console.table(textStatus);
                        console.table(errorThrown);
                    }
                });
            });
            return promiseObj;
        },
        sendFile: function () {
            let self = this;

            let promiseObj = new Promise(function (resolve, reject) {
                if (!self.formAction) {
                    self.showError("Please try again.");
                    resolve(false);
                    return;
                }
                $.ajax({
                    type: self.submitType,
                    url: self.formAction,
                    data: self.jsonData,
                    contentType: false,
                    processData: false,
                    beforeSend: function () {
                        $('#loading_modal').modal("show");
                    },
                    xhr: function () {
                        var xhr = new window.XMLHttpRequest();
                        // Handle progress
                        //Upload progress
                        xhr.upload.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded / evt.total;
                            }
                        }, false);
                        //Download progress
                        xhr.addEventListener("progress", function (evt) {
                            if (evt.lengthComputable) {
                                var percentComplete = evt.loaded / evt.total;
                            }
                        }, false);

                        return xhr;
                    },
                    success: function (response) {
                        $('#loading_modal').modal('hide');
                        if (response.success) {
                            self.responseData = response.data;
                            self.msgType = "success";
                            self.msgTitle = "Success!";
                            self.msg = response.msg;
                            resolve(true);
                        } else {
                            self.msgType = "error";
                            self.msgTitle = "Error!";
                            self.msg = response.errors || response.msg;
                        }
                        if (response.hasOwnProperty('errors') || response.hasOwnProperty('msg')) {
                            if (response.hasOwnProperty('type')) {
                                if (response.type == "Login") {
                                    self.showSessionError(response);
                                }
                            } else {
                                self.showToastrMsg();
                            }

                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $('#loading_modal').modal('hide');
                        console.table(jqXHR.status);
                        console.table(textStatus);
                        console.table(errorThrown);
                    }
                });
            });
            return promiseObj;
        },
        makeSelect: function (id, data) {
            var option = '<option value="">--Please Select--</option>';
            $.each(data, function (i, x) {
                option += '<option value="' + x.value + '">' + x.text + '</option>';
            });
            if (id) {
                $('#' + id).append(option);
            } else {
                return option;
            }
        },
        isObjEmpty: function (obj) {
            for (var key in obj) {
                if (obj.hasOwnProperty(key))
                    return false;
            }
            return true;
        },
        populateToFormInputs: function (data, frmContainer) {
            frmContainer = frmContainer || "";
            $.each(data, function (key, value) {
                value = value == null ? "" : value;
                var type = $("#" + key).attr('type');
                var showIfTrue = "";
                var id = "";
                if (type === undefined) {
                    type = $("input[name='" + key + "']").attr('type');
                }
                if (type === "text" || type === "number" || type === "hidden" || $("#" + key).is("select") || $("#" + key).is("textarea")) {
                    id = $(frmContainer + " [name='" + key + "']").attr('id');
                    if (JSON.stringify(value) == "true") {
                        $("#" + id).val("true");
                    }
                    else if(JSON.stringify(value) == "false") {
                        $("#" + id).val("false");
                    } else {
                        $("#" + id).val($.trim(value)).trigger('change');
                    }
                    
                    if ($("#" + id).hasClass('select2-hidden-accessible')) {
                        $("#" + id).trigger('change.select2');
                        if ($("#" + id).select2("data").length == 0 && value != "" && value != 0) {
                            var Options = new Option(value, value, true, true);
                            $("#" + id).append(Options).trigger('change');
                        }
                    }
                    if ($("#" + id).hasClass('date-format') && value != "") {
                        $("#" + id).datepicker("setDate", new Date(value));
                    }
                    if ($("#" + id).hasClass('number-format')) {
                        $("#" + id).val($Helper().numberFormat(value));
                    }
                    if ($("#" + id).hasClass('decimal-format')) {
                        $("#" + id).val($Helper().DecimalFormat(value));
                    }

                }
                if (type === "radio") {
                    $("input[name='" + key + "']").attr('checked', false);
                    $("input[name='" + key + "'][value='" + value + "']").prop("checked", true);
                    showIfTrue = $("input[name='" + key + "']").data("showiftrue") || "";
                    if (showIfTrue) {
                        if (value == 1) {
                            $(showIfTrue).show();
                        } else {
                            $(showIfTrue).hide();
                        }
                    }
                    $("input[name='" + key + "'][value='" + value + "']").trigger('change');
                }
                if (type == "checkbox") {
                    console.log($("input[name='" + key + "'][value='" + value + "']"))
                    $("input[name='" + key + "']").attr('checked', false);
                    if ($("input[name='" + key + "']").length == 1) {
                        $("input[name='" + key + "']").prop("checked", value == 1);
                        $("input[name='" + key + "']").trigger('change');
                    }
                    else
                    {
                        $("input[name='" + key + "'][value='" + value + "']").prop("checked", true);
                        $("input[name='" + key + "'][value='" + value + "']").trigger('change');
                    }

                }
            });
        },
        getDataTableData: function (tblData) {
            var arrData = [];
            var selectedRowCount = tblData.length;
            var row = "", ID = "";
            for (var i = 0 ; selectedRowCount > i; i++) {
                row = tblData[i];
                arrData.push(row);
            }
            return arrData;
        },
        parsleyValidate: function (form) {
            if ($(".form-control.parsley-error").length) {
                $('#' + form).parsley().validate();
            }
        },
        showSessionError: function (response) {
            $("#iziModalError").iziModal({
                title: "Attention",
                subtitle: response.errors,
                icon: 'icon-power_settings_new',
                headerColor: '#BD5B5B',
                width: 600,
                timeout: 0,
                timeoutProgressbar: true,
                transitionIn: 'fadeInDown',
                transitionOut: 'fadeOutDown',
                pauseOnHover: true
            });
            $('#iziModalError').iziModal('open');
        },
    }
    DataClass.init.prototype = $.extend(DataClass.prototype, $M.init.prototype);

    DataClass.init.prototype = DataClass.prototype;
    return window.DataClass = window.$D = DataClass;
}());
