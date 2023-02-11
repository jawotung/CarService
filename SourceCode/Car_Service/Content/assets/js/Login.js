'use strict';
(function () {
    var Login = $D();
    var Currency = {};
    var data;
       
    $(document).ready(function () {
        GetExchangeRate("USD", "JPY");
        GetExchangeRate("JPY", "USD");
        $("#Username").focus();
        $("#frmLogin").submit(function (e) {
            e.preventDefault();
            LoginMeIn();
        });
        //All Function --------------------------------------------------------------------------------
        function LoginMeIn() {
            Login.formData = $('#frmLogin').serializeArray();
            Login.setJsonData();
            Login.formAction = '/Login/LoginEntry';
            Login.sendData().then(function () {
                var login = Login.responseData;
                if (login.error) {
                    if (login.errmsg == "Invalid Username or Password. Please try again.")
                        Login.showError(login.errmsg);
                    else
                        Login.showErrorNoTimeout(login.errmsg);
                    $("#Username").addClass("input-error");
                    $("#Password").addClass("input-error");
                    $("#Username").addClass("parsley-success");
                    $("#Password").addClass("parsley-success");
                    $("#Password").val("");
                } else {
                    $("#Username").removeClass("input-error");
                    $("#Password").removeClass("input-error");
                    $("#frmLogin > div.login-buttons > button").attr("disabled", true);
                    window.location = "/";
                }
            });
        }

        function GetExchangeRate(fromCurrency, toCurrency) {
            let amountVal = 1;
            let url = `https://v6.exchangerate-api.com/v6/0ae5560ee7661b1824520e41/latest/${fromCurrency}`; // EDIT YOUR-API-KEY (ex: 0ae5560ee7661b1824520e41)

            fetch(url).then(response => response.json()).then(result => {
                let exchangeRate = result.conversion_rates[toCurrency];
                let totalExRate = amountVal * exchangeRate;
                    Currency = {
                        "FromCurrency": fromCurrency,
                        "ToCurrency": toCurrency,
                        "FromCurrencyAmount": amountVal,
                        "ToCurrencyAmount": totalExRate
                    };
                console.log(Currency);
                data = { currency: Currency }
                console.log(data);
                $.ajax({
                    dataType: "json",
                    type: "POST",
                    url: "Login/SaveExchangeRate",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(data),
                });
            }).catch(() => {
               console.log("RATE MASTER : Something went wrong");
                });
        }
    });
})();