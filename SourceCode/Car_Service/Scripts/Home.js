"use strict";
(function () {
    var Customer = $D();
    $(document).ready(function () {
        getCustomerPageInfo();
        $("#registerForm").submit(function (e) {
            e.preventDefault();
            registerNewAccount()
        });
    });
    // FUNCTIONS HERE #################################################################
    function getCustomerPageInfo() {
        Customer.formAction = "/Home/GetCustomerPageInfo";
        Customer.sendData().then(function () {
            var listOfPath = Customer.responseData;
            if (listOfPath.length <= 0)
                return;

            var html = "";
            for (var i = 0; i < listOfPath.length; i++) {
                switch (i) {
                    case 0:
                        $("#imgPortFolio_1").attr("src", "/Content/assets/img/carservices/" + listOfPath[i]);
                        break;
                    default:
                        html += '<div class="col-lg-4 col-sm-6 mb-4">' +
                                    '<div class="portfolio-item">' +
                                        '<a class="portfolio-link" data-bs-toggle="modal" href="#portfolioModal1">' +
                                            '<div class="portfolio-hover">' +
                                                '<div class="portfolio-hover-content"><i class="fas fa-plus fa-3x"></i></div>' +
                                            '</div>'+ 
                                            '<img id="imgPortFolio_' + (i + 1) + '"  class="img-fluid" src="/Content/assets/img/carservices/' + listOfPath[i] + '" alt="..." />' +
                                        '</a>' +
                                        '<div class="portfolio-caption">' +
                                            '<div class="portfolio-caption-heading">Threads</div>' +
                                            '<div class="portfolio-caption-subheading text-muted">Illustration</div>' +
                                        '</div>' +
                                    '</div>' +
                                '</div>';
                        break;
                }
            }
            $("#portfolioRow").append(html);
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
})();

