"use strict";
(function () {
    var ajax = $D();
    var tblOngoing = "";
    var $H = $Helper();
    $(document).ready(function () {
        drawDatatables();
        $('#tblOngoing tbody').on('click', 'tr', function (e) {
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
                    var data = tblOngoing.row($(this)).data();
                    if ($.trim(data) != "") {
                        if ($(this).hasClass('selected')) {
                            Edit();
                        }
                        else {
                            tblOngoing.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            $('#btnEdit').removeAttr("disabled");
                            $('#btnDelete').removeAttr("disabled");
                        }
                    }
                    break;
            }
        });
        $("#tblOngoing").on("change", '.columnSearch', function () {
            tblOngoing.ajax.reload(null, false);
        });
        $("#tblOngoing").on("click", ".btnCompleted", function () {
            var data = tblOngoing.row($(this).parents('tr')).data();
            ajax.msg = "Are you sure this service is completed?";
            ajax.confirmAction().then(function (approve) {
                if (approve) {
                    ajax.formAction = '/Transaction/OngoingService/CompleteService';
                    ajax.jsonData = { ID: data.JODetailID };
                    ajax.sendData().then(function () {
                        tblOngoing.ajax.reload(null, false);
                    });
                }
            });
        });

    });

    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblOngoing')) {
            tblOngoing = $('#tblOngoing').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/Transaction/OngoingService/GetOngoingServiceList",
                    "type": "GET",
                    "datatype": "json",
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "JONo", data: "JONo" },
                    { title: "UserID", data: 'UserID' },
                    { title: "Customer FullName", data: 'FullName' },
                    { title: "Worker", data: 'Worker' },
                    { title: "Service", data: 'ServiceName', className:"font-weight-bold" },
                    { title: "Startdate", data: 'Startdate' },
                    { title: "Estemate Enddate", data: 'Enddate' },
                    {
                        title: "", data: function (data) {
                            return '<button type="button" class="btn btn-sm btn-green btn-block btnCompleted">Completed</button>'
                        }
                    },
                ],
            })
        }
    }
})();
