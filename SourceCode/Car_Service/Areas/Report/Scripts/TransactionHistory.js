"use strict";
(function () {
    var ajax = $D();
    var tblTransactionHistory = "";
    var $H = $Helper();
    $(document).ready(function () {
        $("#StartDate").datepicker("setDate", new Date($H.DateMonthStart()));
        $("#EndDate").datepicker("setDate", new Date($H.DateMonthEnd()));
        drawDatatables();

        $('#tblTransactionHistory tbody').on('click', 'tr', function (e) {
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
                    var data = tblTransactionHistory.row($(this)).data();
                    if ($.trim(data) != "") {
                        if ($(this).hasClass('selected')) {
                            Edit();
                        }
                        else {
                            tblTransactionHistory.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            $('#btnEdit').removeAttr("disabled");
                            $('#btnDelete').removeAttr("disabled");
                        }
                    }
                    break;
            }
        });
        $("#tblTransactionHistory").on("change", '.columnSearch', function () {
            tblTransactionHistory.ajax.reload(null, false);
        });
    });

    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblTransactionHistory')) {
            tblTransactionHistory = $('#tblTransactionHistory').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/Report/TransactionHistory/GetTransactionHistoryList",
                    "type": "GET",
                    "datatype": "json",
                    "data": function (d) {
                        d["StartDate"] = $("#StartDate").val();
                        d["EndDate"] = $("#EndDate").val();
                    }
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "JONo", data: "JONo" },
                    { title: "UserID", data: 'UserID' },
                    { title: "Customer FullName", data: 'FullName' },
                    { title: "Worker", data: 'Worker' },
                    { title: "Service", data: 'ServiceName' },
                    { title: "Price", data: 'Price' },
                    { title: "Startdate", data: 'Startdate' },
                    { title: "Enddate", data: 'Enddate' },
                ],
            })
        }
    }
})();
