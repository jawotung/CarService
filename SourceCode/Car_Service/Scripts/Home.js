"use strict";
(function () {
    const sampleServices = ["Replace air filter", "Wiper blades replacement", "Oil filter changed", "Scheduled maintenance"
        , "New tires", "Battery replacement", "Brake work", "Antifreeze added"];
    var Customer = $D();
    var selectedServices = [];
    var tblCustomerService = "";
    $(document).ready(function () {
        initializePage();
        $("#registerForm").submit(function (e) {
            e.preventDefault();
            registerNewAccount()
        });
        $("#frmCustomerServiceForm").submit(function (e) {
            e.preventDefault();
            registerNewAccount()
        });
        $("#btnServiceCart").click(function (e) {
            var selectedSetvicesCount = $("#divServiceRow").find(".service-selected").length;
            if (selectedSetvicesCount <= 0) {
                Customer.showWarning("Please select atleast 1 service(s)."); // Uncomment this once plugins fixed
                //alert("Please select atleast 1 service(s).");
                return;
            }
            $("#mdlCustomerServiceForm").modal("show");
            drawDatatables();
        });
    });
    // FUNCTIONS HERE #################################################################
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
                    { title: "No #", data: "ID", width: "7%" },
                    { title: "ServiceName", data: "ServiceName", width: "80%" },
                    /*
                    {
                        title: "Action", data: function (data) {
                            return '<button type="button" class="btn btn-sm btn-danger btnRemove"><i class="fa fa-trash"></i></button>';
                        }, width: "8%"
                    },
                    */
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
    function getCustomerPageInfo() {
        Customer.formAction = "/Home/GetCustomerPageInfo";
        Customer.sendData().then(function () {
            var listOfPath = Customer.responseData;
            if (listOfPath.length <= 0)
                return;

            var html = "";
            for (var i = 0; i < listOfPath.length; i++) {
                html += '<div class="col-lg-4 col-sm-6 mb-4">' +
                            '<div class="portfolio-item" data-id="' + (i + 1) + '">' + // Temporary id due to no DB yet
                                '<a class="portfolio-link" data-bs-toggle="modal" href="#portfolioModal1">' +
                                    '<div class="portfolio-hover">' +
                                        '<div class="portfolio-hover-content"><i class="fas fa-plus fa-3x"></i></div>' +
                                    '</div>'+ 
                                    '<img id="imgPortFolio_' + (i + 1) + '"  class="img-fluid" src="/Content/assets/img/carservices/' + listOfPath[i] + '" alt="..." />' +
                                '</a>' +
                                '<div class="portfolio-caption">' +
                                    '<div class="portfolio-caption-heading">' + sampleServices[i] + '</div>' +
                                    '<div class="portfolio-caption-subheading text-muted">Illustration</div>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
            }
            $("#divServiceRow").append(html);

            $(".portfolio-hover").click(function () {
                var $el = $(this);
                var dataID = $el.closest("div.portfolio-item").attr("data-id");
                if (!$el.closest("div.portfolio-item").hasClass("service-selected")) {
                    $el.closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#ffc800");
                    $el.closest("div.portfolio-item").css("border", "2px solid #ffc800");
                    $el.closest("div.portfolio-item").addClass("service-selected");

                    selectedServices.push({
                        ID: dataID,
                        ServiceName: sampleServices[parseInt(dataID) - 1]
                    });
                } else {
                    $el.closest("a.portfolio-link").next("div.portfolio-caption").css("background", "#fff");
                    $el.closest("div.portfolio-item").css("border", "");
                    $el.closest("div.portfolio-item").removeClass("service-selected");

                    selectedServices = selectedServices.filter(x => x.ID != parseInt(dataID));
                }
            });
        });
    }
    function registerNewAccount() {
        Customer.formData = $('#registerForm').serializeArray();
        Customer.formAction = '/Home/SaveCustomer';
        Customer.setJsonData();
        Customer.sendData().then(function () {
            $(".form-control").val("");
        });
    }
    function saveOnlineJobOrder() {
        Customer.formData = $('#frmCustomerServiceForm').serializeArray();
        Customer.formAction = '/Home/SaveOnlineJobOrder';
        Customer.setJsonData();
        Customer.sendData().then(function () {
            tblCustomerService.ajax.reload(false);
            cancelForm();
            cancelTbl();
        });
    }
    function cancelForm() {
        Customer.clearFromData("frmCustomerServiceForm");
        $('#CustomerServiceFormID').prop('readonly', false);
        $("#CustomerServiceFormTitle").text(" Customer Service Form");
        $("#btnSave .btnLabel").text(" Save");
        $("#mdlCustomerServiceForm").modal("hide");
    }
    function cancelTbl() {
        $('#btnEdit').attr("disabled", "disabled");
        $('#btnDelete').attr("disabled", "disabled");
    }
})();

