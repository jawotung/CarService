"use strict";
(function () {
    const sampleServices = ["Replace air filter", "Wiper blades replacement", "Oil filter changed", "Scheduled maintenance"
        , "New tires", "Battery replacement", "Brake work", "Antifreeze added"];
    var Customer = $D();
    var Formatter = $F();
    var selectedServices = [];
    var tblCustomerService = "";
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
        $("#LoginLink").click(function (e) {
            e.preventDefault();
            $("#mdlLoginForm").modal("show");
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
        if (!$.fn.DataTable.isDataTable('#tblCustomerServiceForm')) {
            tblCustomerService= $('#tblCustomerServiceForm').DataTable({
                searching: false,
                data: selectedServices,
                dataSrc: "data",
                select: true,
                sorting: false,
                ordering: false,
                columns: [
                    { title: "No #", data: "ServiceID", width: "10%" },
                    { title: "ServiceName", data: "ServiceName", width: "80%" },
                    {
                        title: "Amount", data: function (data) {
                            Formatter.dataToFormat = +data.Price;
                            return Formatter.formatDecimal(0);
                        }, width: "10%"
                    }
                ],
                initComplete: function () {
                    $("#tblCustomerServiceForm tbody").on("click", "tr", function (e) {
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
                                var data = tblCustomerService.row($(this)).data();
                                if ($.trim(data) != "") {
                                    if ($(this).hasClass('selected')) {
                                        Edit();
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
                                    '</div>'+ 
                                    '<img id="imgPortFolio_' + (data[i] + 1) + '"  class="img-fluid" src="/Content/assets/img/carservices/' + listOfPath[i] + '" alt="..." />' +
                                '</a>' +
                                '<div class="portfolio-caption">' +
                                    '<div class="portfolio-caption-heading">' + data[i].ServiceName + '</div>' +
                                    '<div class="portfolio-caption-subheading text-muted">P ' + serviceAmount + '</div>' +
                                    '<div class="portfolio-caption-subheading text-muted">' + data[i].DurationFrom  + ' ~ ' + data[i].DurationTo + '</div>' +
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
    function saveOnlineJobOrder() {
        var headerData = flSerializeToArray($('#frmCustomerServiceForm').serializeArray());
        Customer.formAction = '/Home/SaveOnlineJobOrder';
        Customer.jsonData = {
            HeaderData: headerData,
            ServiceDetail: selectedServices
        };
        Customer.sendData().then(function () {
            cancelForm();
            $(".portfolio-hover").closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#ffff")
            $(".portfolio-hover").closest("div.portfolio-item").css("border", "");
            tblCustomerService.ajax.reload(false);
            $("#mdlLoginForm").modal("hide");
        });
    }
})();

