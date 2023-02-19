"use strict";
(function () {
    var ajax = $D();
    var tblAssignWorker = "";
    var $H = $Helper();
    $(document).ready(function () {
        drawDatatables();

        
        $("#tblAssignWorker tbody").on("click", '.btnAssign', function () {
            tblAssignWorker.ajax.reload(null, false);
        });

        $("#btnSave").click(function (e) {
            e.preventDefault();
            ajax.formAction = '/Transaction/AssignWorker/DeleteWorker';
            ajax.jsonData = { ID: data.ID };
            ajax.sendData().then(function () {
                tblWorker.ajax.reload(null, false);
                cancelTbl();
                cancelForm();
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
                    "url": "/MasterMaintenance/ServiceMaster/GetServiceList",
                    "type": "GET",
                    "datatype": "json",
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "Service", data: "Service" },
                    {
                        title: "Worker", data: function () {
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
                                    sp: "EXEC tAssignWorker_Worker @Startdate = '" + $(this).attr("data-Startdate") + "'",
                                };
                            },
                        },
                        placeholder: '-Please Select-',
                        theme: 'bootstrap4',
                        width: 'resolve'
                    });
                    LoadInputs();
                },
            })
        }
    }
})();
