"use strict";
(function () {
    var ajax = $D();
    var tblService = "";
    var $H = $Helper();
    $(document).ready(function () {
        drawDatatables();

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

        $('#tblService tbody').on('click', 'tr', function (e) {
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
                    var data = tblService.row($(this)).data();
                    if ($.trim(data) != "") {
                        if ($(this).hasClass('selected')) {
                            Edit();
                        }
                        else {
                            tblService.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            $('#btnEdit').removeAttr("disabled");
                            $('#btnDelete').removeAttr("disabled");
                        }
                    }
                    break;
            }
        });
        $("#tblService").on("change", '.columnSearch', function () {
            tblService.ajax.reload(null, false);
        });

        $("#btnAdd").click(function () {
            $("#mdlService").modal("show");
            tblService.ajax.reload(null, false);
            cancelTbl();
            cancelForm();
        });
        $('#btnEdit').click(function () {
            Edit();
        });
        $('#btnDelete').click(function () {
            var data = tblService.rows('.selected').data()[0];
            ajax.msg = "Are you sure you want to delete this Service?";
            ajax.confirmAction().then(function (approve) {
                if (approve) {
                    ajax.formAction = '/MasterMaintenance/ServiceMaster/DeleteService';
                    ajax.jsonData = { ID: data.ID };
                    ajax.sendData().then(function () {
                        tblService.ajax.reload(null, false);
                        cancelTbl();
                        cancelForm();
                    });
                }
            });
        });
        $("#frmService").submit(function (e) {
            e.preventDefault();
            ajax.formData = $('#frmService').serializeArray();
            ajax.formAction = '/MasterMaintenance/ServiceMaster/SaveService';
            ajax.setJsonData().sendData().then(function () {
                tblService.ajax.reload(false);
                cancelTbl();
                cancelForm();
            });
        });
    });

    function drawDatatables() {
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
                    { title: "ServiceName", data: "ServiceName" },
                    { title: "Duration", data: 'Duration' },
                    { title: "Amount", data: 'Amount' },
                    { title: "Position", data: 'PositionName' },
                ],
            })
        }
    }
    function Edit() {
        var data = tblService.rows('.selected').data()[0];
        $("#frmService").parsley().reset();
        $("#mdlServiceTitle").text(" Update Service");
        $('#ServiceName').prop('readonly', true);
        $("#btnSave .btnLabel").text(" Update");
        ajax.populateToFormInputs(data, "#frmService");
        $("#mdlService").modal("show");
    }
    function cancelForm() {
        ajax.clearFromData("frmService");
        $('#ServiceName').prop('readonly', false);
        $("#mdlServiceTitle").text(" Create Service");
        $("#btnSave .btnLabel").text(" Save");
        $("#mdlService").modal("hide");
    }
    function cancelTbl() {
        $('#btnEdit').attr("disabled", "disabled");
        $('#btnDelete').attr("disabled", "disabled");
    }
})();
