"use strict";
(function () {
    var ajax = $D();
    var tblAssignWorker = "";
    var tblService = "";
    var $H = $Helper();
    var HeaderID = 0;
    $(document).ready(function () {
        drawDatatables();
        $("#tblAssignWorker tbody").on("click", '.btnAssign', function () {
            tblAssignWorker.ajax.reload(null, false);
        });
        $("#tblAssignWorker").on("click", ".btnAssign", function () {
            var data = tblAssignWorker.row($(this).parents('tr')).data();
            HeaderID = data.ID;
            tblService.ajax.reload(null, false);
            $("#mdlAssignWorker").modal("show");
        });
        $("#btnSave").click(function (e) {
            e.preventDefault();
            var data = [];
            tblService.rows().every(function (index, element) {
                var row = $(this.node());
                data.push({
                    JODetailID: tblService.row(this).data().JODetailID,
                    WorkerID: row.find('.WorkerID').val(),
                });
            });
            ajax.formAction = '/Transaction/AssignWorker/SaveWalkIn';
            ajax.jsonData = { data: data };
            ajax.sendData().then(function () {
                tblAssignWorker.ajax.reload(null, false);
                tblService.ajax.reload(null, false);
                $("#mdlAssignWorker").modal("hide");
            });
        });
    });

    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblAssignWorker')) {
            tblAssignWorker = $('#tblAssignWorker').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/Transaction/AssignWorker/GetAssignWorkerList",
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
                    {
                        title: "", data: function (data) {
                            return '<button type="button" class="btn btn-sm btn-green btn-block btnAssign">Assign</button>'
                        }
                    },
                ],
            })
        }
        if (!$.fn.DataTable.isDataTable('#tblService')) {
            tblService = $('#tblService').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/Transaction/AssignWorker/GetServiceList",
                    "type": "GET",
                    "datatype": "json",
                    "data": function (d) {
                        d.ID = HeaderID;
                    }
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "Service", data: "ServiceName" },
                    {
                        title: "Worker", data: function (data) {
                            return "<select type='text' class='form-control input WorkerID' style='width: 100%'  data-Startdate='" + data.Startdate +"' autocomplete='off'/>";
                        }
                    },
                ],
                fnDrawCallback: function () {

                    $('.WorkerID').select2({
                        ajax: {
                            url: "/General/GetSelect2SP",
                            data: function (params) {
                                return {
                                    q: params.term,
                                    sp: "EXEC tAssignWorker_Worker @Startdate = '" + $(this).attr("data-Startdate") + "', @Search = '" + params.term + "'",
                                    db: "CarService",
                                };
                            },
                        },
                        placeholder: '-Please Select-',
                        theme: 'bootstrap4',
                        width: 'resolve'
                    });
                    $H.LoadInputs();
                },
            })
        }
    }
})();
