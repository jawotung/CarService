"use strict";
(function () {
    var ajax = $D();
    var tblWalkIn = "";
    var tblService = "";
    var $H = $Helper();
    var IsNewCustomer = true;
    $(document).ready(function () {
        drawDatatables();

        $('#tblWalkIn tbody').on('click', 'tr', function (e) {
            switch (e.target.localName) {
                case "button":
                    break;
                case "span":
                    break;
                case "checkbox":
                    break;
                case "i":
                    break;
                case "textbox":
                    break;
                case "input":
                    break;
                default:
                    var data = tblWalkIn.row($(this)).data();
                    if ($.trim(data) != "") {
                        if ($(this).hasClass('selected')) {
                            Edit();
                        }
                        else {
                            tblWalkIn.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            $('#btnEdit').removeAttr("disabled");
                            $('#btnDelete').removeAttr("disabled");
                        }
                    }
                    break;
            }
        });
        $("#tblWalkIn").on("change", '.columnSearch', function () {
            tblWalkIn.ajax.reload(null, false);
        });

        $('#btnGo').click(function () {
            if (!IsNewCustomer) {
                ajax.formAction = '/Transaction/WalkIn/GetCustomer';
                ajax.jsonData = { ID: $(this).val() };
                ajax.sendData().then(function () {
                    ajax.populateToFormInputs(ajax.responseData, "#frmWalkIn");
                });
            }
        });
        $('#btnNewCustomer').click(function () {
            if (IsNewCustomer) {
                IsNewCustomer = false;
                $(".isNewCusomre").attr("readonly", "true");
                $("#btnNewCustomerLabel").text("Create new account")
                $('#btnGo').show();
            } else {
                IsNewCustomer = true;
                $(".isNewCusomre").removeAttr("readonly");
                $("#btnNewCustomerLabel").text("Have an account")
                $('#btnGo').hide();
            }
        });
        $("#btnAdd").click(function () {
            $("#mdlWalkIn").modal("show");
            tblWalkIn.ajax.reload(null, false);
            cancelTbl();
            cancelForm();
        });
        $('#btnEdit').click(function () {
            Edit();
        });
        $('#btnDelete').click(function () {
            var data = tblWalkIn.rows('.selected').data()[0];
            ajax.msg = "Are you sure you want to delete this data?";
            ajax.confirmAction().then(function (approve) {
                if (approve) {
                    ajax.formAction = '/Transaction/WalkIn/DeleteWalkIn';
                    ajax.jsonData = { ID: data.ID };
                    ajax.sendData().then(function () {
                        tblWalkIn.ajax.reload(null, false);
                        cancelTbl();
                        cancelForm();
                    });
                }
            });
        });
        $("#frmWalkIn").submit(function (e) {
            e.preventDefault();
            if ($(".CheckItem:checked").length != 0) {
                var data = $H.serializeToArray($('#frmWalkIn').serializeArray());
                var Detail = [];
                $.each($(".CheckItem:checked"), function () {
                    Detail.push({
                        ServiceID: $(this).attr("data-ID"),
                        Price: $(this).attr("data-Amount"),
                    });
                });
                ajax.formAction = '/Transaction/WalkIn/SaveWalkIn';
                ajax.jsonData = {
                    data: data,
                    Detail: Detail,
                    IsNewCustomer: IsNewCustomer,
                };
                ajax.sendData().then(function () {
                    cancelForm(); cancelTbl();
                });
            } else {
                ajax.showError("Please check the checkbox in the table");
            }
        });
        $("#FirstName, #MiddleName, #LastName").blur(function () {
            $(this).val($Helper().MakeFirstLetterUpper($(this).val()));
        });
    });

    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblWalkIn')) {
            tblWalkIn = $('#tblWalkIn').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/Transaction/WalkIn/GetWalkInList",
                    "type": "GET",
                    "datatype": "json",
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "JONo", data: "JONo" },
                    { title: "UserID", data: 'UserID' },
                    { title: "FullName", data: 'FullName' },
                    { title: "Service", data: 'ServiceName' },
                ],
            })
        }
        if (!$.fn.DataTable.isDataTable('#tblService')) {
            tblService = $('#tblService').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/MasterMaintenance/ServiceMaster/GetServiceList",
                    "type": "GET",
                    "datatype": "json",
                },
                dataSrc: "data",
                select: true,
                columns: [
                    {
                        title: '',
                        data: function (data) {
                            return '<input type="checkbox" data-ID="' + data.ID + '" data-Amount="' + data.Amount + '" class="CheckItem" />';
                        }, sortable: false, orderable: false, width: "5%"
                    },
                    { title: "ServiceName", data: "ServiceName" },
                    { title: "Duration", data: 'Duration' },
                    { title: "Amount", data: 'Amount' },
                    { title: "Position", data: 'PositionName' },
                ],
            })
        }
    }
    function Edit() {
        var data = tblWalkIn.rows('.selected').data()[0];
        $("#frmWalkIn").parsley().reset();
        $("#mdlWalkInTitle").text(" Update WalkIn");
        $('#WalkInID').prop('readonly', true);
        $("#btnSave .btnLabel").text(" Update");
        ajax.populateToFormInputs(data, "#frmWalkIn");
        $("#Password").val("");
        $("#Password").attr('required', false);
        $("#mdlWalkIn").modal("show");
    }
    function cancelForm() {
        IsNewCustomer = true;
        $(".isNewCusomre").removeAttr("readonly");
        $("#btnNewCustomerLabel").text("Have an account")
        $('#btnGo').hide();
        $('#WalkInID').prop('readonly', false);
        ajax.clearFromData("frmWalkIn");
        $("#mdlWalkInTitle").text(" Create WalkIn");
        $("#btnSave .btnLabel").text(" Save");
        $("#Password").val("");
        $("#Password").attr('required', true);
        $("#mdlWalkIn").modal("hide");
    }
    function cancelTbl() {
        $('#btnEdit').attr("disabled", "disabled");
        $('#btnDelete').attr("disabled", "disabled");
        tblWalkIn.ajax.reload(null, false);
        tblService.ajax.reload(null, false);
    }
})();
