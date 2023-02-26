"use strict";
(function () {
    var Customer = $D();
    var Formatter = $F();
    var selectedServices = [];
    var tblCustomerService = "";
    var tblCustomerTransHistory = "";
    $(document).ready(function () {
        initializePage();
        $("#btnServiceCart").click(function (e) {
            var selectedSetvicesCount = $("#divServiceRow").find(".service-selected").length;
            if (selectedSetvicesCount <= 0) {
                Customer.showWarning("Please select atleast 1 service(s).");
                return;
            }
            $("#mdlCustomerServiceForm").modal("show");
            drawDatatables();
        });
        $("#frmCustomerServiceForm").submit(function (e) {
            e.preventDefault();
            saveOnlineJobOrder()
        });
        $("#frmLoginForm").submit(function (e) {
            e.preventDefault();
            logMeIn();
        });
        $("#LoginLink").click(function (e) {
            e.preventDefault();
            $("#mdlLoginForm").modal("show");
        });
        $("#TransactionHistLink").click(function (e) {
            e.preventDefault();
            $("#mdlTransHistory").modal("show");
            drawTransHistoryDataTable();
        });
    });
    // FUNCTIONS HERE #################################################################
    function cancelForm() {
        selectedServices = [];
        Customer.clearFromData("frmCustomerServiceForm");
        $('#CustomerServiceFormID').prop('readonly', false);
        $("#CustomerServiceFormTitle").text(" Customer Service Form");
        $("#btnSave .btnLabel").text(" Save");
        $("#mdlCustomerServiceForm").modal("hide");
    }
    function drawDatatables() {
        tblCustomerService = $('#tblCustomerServiceForm').DataTable({
            searching: false,
            destroy: true,
            data: selectedServices,
            dataSrc: "data",
            select: true,
            sorting: false,
            ordering: false,
            columns: [
                { title: "No #", data: "ServiceID", width: "10%" },
                { title: "ServiceName", data: "ServiceName", width: "75%" },
                {
                    title: "Amount", data: function (data) {
                        Formatter.dataToFormat = +data.Price;
                        return Formatter.formatDecimal(0);
                    }, width: "10%"
                },
                {
                    title: "", data: function (data) {
                        return '<button type="button" class="btn btn-sm btn-danger btn-block btnAssign" data-id=' + data.ServiceID + '>Remove</button>'
                    }, width: "5%"
                },
            ],
            initComplete: function () {
                $("#tblCustomerServiceForm tbody").on("click", "tr", function (e) {
                    switch (e.target.localName) {
                        case "button":
                            var serviceID = $(e.target).attr("data-id");
                            selectedServices = selectedServices.filter(x => x.ServiceID != parseInt(serviceID));
                            $(this).remove();

                            $("#divServiceRow .service-selected").each(function () {
                                var thizServiceID = $(this).attr("data-id");
                                if (+serviceID == +thizServiceID) {
                                    $(this).find("div.portfolio-hover").closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#ffff");
                                    $(this).find(".portfolio-hover").closest("div.portfolio-item").css("border", "");
                                    $(this).find(".service-selected").removeClass("service-selected");
                                }
                            });
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
                            var data = tblCustomerService.row($(this)).data();
                            if ($.trim(data) != "") {
                                if ($(this).hasClass('selected')) {
                                    
                                }
                                else {
                                    tblCustomerService.$('tr.selected').removeClass('selected');
                                    $(this).addClass('selected');
                                    $('#btnEdit').removeAttr("disabled");
                                    $('#btnDelete').removeAttr("disabled");
                                }
                            }
                            break;
                    }
                });
                $("#tblCustomerServiceForm").on("change", '.columnSearch', function () {
                    tblCustomerService.ajax.reload(null, false);
                });
            }
        })

        // Get Customer Personal Details
        var localID = +$("#ID").val() || 0;
        if (localID > 0) {
            Customer.jsonData = { ID: +$("#ID").val() || 0 };
            Customer.formAction = '/Home/GetCustomerDetails';
            Customer.sendData().then(function () {
                var data = Customer.responseData;
                if (data != null) {
                    Customer.populateToFormInputs(data, "#frmCustomerServiceForm");
                    $(".input-to-disable").attr("readonly", "readonly");
                }
                else {
                    $(".input-to-disable").removeAttr("readonly");
                }
            });
        }
        else {
            $(".input-to-disable").removeAttr("disabled");
        }
    }
    function drawTransHistoryDataTable() {
        if (!$.fn.DataTable.isDataTable('#tblTransHistory')) {
            tblCustomerTransHistory = $('#tblTransHistory').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/Home/GetJobOrderListByUserID",
                    "type": "GET",
                    "datatype": "json",
                    "data": function (d) {
                        d["ID"] = +$("#ID").val()
                    }
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "#", data: "Row_Num", width: "5%" },
                    { title: "JONo", data: 'JONo', width: "15%" },
                    { title: "Start Date", data: 'Startdate', width: "10%" },
                    { title: "Service", data: 'ServiceName', width: "25%" },
                    { title: "Remarks", data: 'Remarks', width: "30%" },
                    { title: "Registered Date", data: 'CreateDate', width: "15%" },
                ],
            })
        }
    }
    function initializePage() {
        $('#Position').select2({
            ajax: {
                url: "/General/GetSelect2Data",
                data: function (params) {
                    return {
                        q: params.term,
                        id: 'ID',
                        text: 'Value',
                        table: 'mGeneral',
                        db: 'CarService',
                        condition: ' AND IsDeleted=0 AND TypeID = 1',
                        display: 'id&text',
                    };
                },
            },
            placeholder: '-Please Select-',
            theme: 'bootstrap4',
            width: 'resolve'
        });
        getCustomerPageInfo();
    }
    function flSerializeToArray(formArray) {
        var returnArray = {};
        for (var i = 0; i < formArray.length; i++) {
            returnArray[formArray[i]['name']] = formArray[i]['value'];
        }
        return returnArray;
    }
    function getCustomerPageInfo() {
        Customer.formAction = "/Home/GetServices";
        Customer.sendData().then(function () {
            var listOfPath = ["1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jfif", "6.jpg", "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg"];
            var data = Customer.responseData;
            if (data.length <= 0)
                return;

            var html = "";
            for (var i = 0; i < data.length; i++) {
                Formatter.dataToFormat = data[i].Amount;
                var serviceAmount = Formatter.formatDecimal(0);
                html += '<div class="col-lg-4 col-sm-6 mb-4">' +
                    '<div class="portfolio-item" data-id="' + data[i].ID + '" data-servicename="' + data[i].ServiceName + '" data-amount="' + data[i].Amount + '">' + // Temporary id due to no DB yet
                    '<a class="portfolio-link" data-bs-toggle="modal" href="#portfolioModal1">' +
                    '<div class="portfolio-hover">' +
                    '<div class="portfolio-hover-content"><i class="fas fa-plus fa-3x"></i></div>' +
                    '</div>' +
                    '<img id="imgPortFolio_' + (data[i] + 1) + '"  class="img-fluid" src="/Content/assets/img/carservices/' + listOfPath[i] + '" alt="..." />' +
                    '</a>' +
                    '<div class="portfolio-caption">' +
                    '<div class="portfolio-caption-heading">' + data[i].ServiceName + '</div>' +
                    '<div class="portfolio-caption-subheading text-muted">P ' + serviceAmount + '</div>' +
                    '<div class="portfolio-caption-subheading text-muted">' + data[i].DurationFrom + ' ~ ' + data[i].DurationTo + '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>';
            }
            $("#divServiceRow").append(html);

            $(".portfolio-hover").click(function () {
                var $el = $(this)
                    , $divPortItem = $el.closest("div.portfolio-item");

                var serviceID = $divPortItem.attr("data-id")
                    , serviceName = $divPortItem.attr("data-servicename")
                    , amount = $divPortItem.attr("data-amount");

                if (!$el.closest("div.portfolio-item").hasClass("service-selected")) {
                    $el.closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#ffc800");
                    $divPortItem.css("border", "2px solid #ffc800");
                    $divPortItem.addClass("service-selected");

                    selectedServices.push({
                        ServiceID: serviceID,
                        ServiceName: serviceName,
                        Price: amount
                    });
                } else {
                    $el.closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#fff");
                    $divPortItem.css("border", "");
                    $divPortItem.removeClass("service-selected");

                    selectedServices = selectedServices.filter(x => x.ServiceID != parseInt(serviceID));
                }
            });
        });
    }
    function logMeIn() {
        Customer.formData = $('#frmLoginForm').serializeArray();
        Customer.setJsonData();
        Customer.formAction = '/Home/LoginMeIn';
        Customer.sendData().then(function () {
            var customer = Customer.responseData;
            if (customer.error) {
                if (customer.errmsg == "Invalid UserID or Password. Please try again.")
                    Customer.showError(customer.errmsg);
                else
                    Customer.showErrorNoTimeout(customer.errmsg);
                $("#LoginUserID").addClass("input-error");
                $("#LoginPassword").addClass("input-error");
                $("#LoginUserID").addClass("parsley-success");
                $("#LoginPassword").addClass("parsley-success");
                $("#LoginPassword").val("");
            } else {
                $("#LoginUserID").removeClass("input-error");
                $("#LoginPassword").removeClass("input-error");
                $("#frmLoginForm > div.login-buttons > button").attr("disabled", true);
                window.location = "/";
            }
        });
    }
    function saveOnlineJobOrder() {
        if (selectedServices.length <= 0) {
            Customer.showError("Please atleast select one services.");
            return;
        }
        var headerData = flSerializeToArray($('#frmCustomerServiceForm').serializeArray());
        Customer.formAction = '/Home/SaveOnlineJobOrder';
        Customer.jsonData = {
            HeaderData: headerData,
            ServiceDetail: selectedServices,
            IsNewCustomer: $("#ID").val().toString() == "0" ? 1 : 0
        };
        Customer.sendData().then(function () {
            cancelForm();
            $(".portfolio-hover").closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#ffff");
            $(".portfolio-hover").closest("div.portfolio-item").css("border", "");
            $("#divServiceRow").find(".service-selected").removeClass("service-selected");
            $("#mdlLoginForm").modal("hide");
        });
    }
})();

