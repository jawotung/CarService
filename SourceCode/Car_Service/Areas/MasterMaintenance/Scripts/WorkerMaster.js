"use strict";
(function () {
    var ajax = $D();
    var tblWorker = "";
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

        $('#tblWorker tbody').on('click', 'tr', function (e) {
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
                    var data = tblWorker.row($(this)).data();
                    if ($.trim(data) != "") {
                        if ($(this).hasClass('selected')) {
                            Edit();
                        }
                        else {
                            tblWorker.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            $('#btnEdit').removeAttr("disabled");
                            $('#btnDelete').removeAttr("disabled");
                        }
                    }
                    break;
            }
        });
        $("#tblWorker").on("change", '.columnSearch', function () {
            tblWorker.ajax.reload(null, false);
        });

        $("#btnAdd").click(function () {
            $("#mdlWorker").modal("show");
            tblWorker.ajax.reload(null, false);
            cancelTbl();
            cancelForm();
        });
        $('#btnEdit').click(function () {
            Edit();
        });
        $('#btnDelete').click(function () {
            var data = tblWorker.rows('.selected').data()[0];
            ajax.msg = "Are you sure you want to delete this Worker?";
            ajax.confirmAction().then(function (approve) {
                if (approve) {
                    ajax.formAction = '/MasterMaintenance/WorkerMaster/DeleteWorker';
                    ajax.jsonData = { ID: data.ID };
                    ajax.sendData().then(function () {
                        tblWorker.ajax.reload(null, false);
                        cancelTbl();
                        cancelForm();
                    });
                }
            });
        });
        $("#frmWorker").submit(function (e) {
            e.preventDefault();
            ajax.formData = $('#frmWorker').serializeArray();
            ajax.formAction = '/MasterMaintenance/WorkerMaster/SaveWorker';
            ajax.setJsonData().sendData().then(function () {
                tblWorker.ajax.reload(false);
                cancelTbl();
                cancelForm();
            });
        });
        $("#FirstName, #MiddleName, #LastName").blur(function () {
            $(this).val($Helper().MakeFirstLetterUpper($(this).val()));
        });
    });

    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblWorker')) {
            tblWorker = $('#tblWorker').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/MasterMaintenance/WorkerMaster/GetWorkerList",
                    "type": "GET",
                    "datatype": "json",
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "WorkerID", data: "WorkerID" },
                    { title: "FullName", data: 'FullName' },
                    { title: "Position", data: 'PositionName' },
                ],
            })
        }
    }
    function Edit() {
        var data = tblWorker.rows('.selected').data()[0];
        $("#frmWorker").parsley().reset();
        $("#mdlWorkerTitle").text(" Update Worker");
        $('#WorkerID').prop('readonly', true);
        $("#btnSave .btnLabel").text(" Update");
        ajax.populateToFormInputs(data, "#frmWorker");
        $("#Password").val(data.Password);
        $("#Password").attr('required', false);
        $("#mdlWorker").modal("show");
    }
    function cancelForm() {
        ajax.clearFromData("frmWorker");
        $('#WorkerID').prop('readonly', false);
        $("#mdlWorkerTitle").text(" Create Worker");
        $("#btnSave .btnLabel").text(" Save");
        $("#Password").val("");
        $("#Password").attr('required', true);
        $("#mdlWorker").modal("hide");
    }
    function cancelTbl() {
        $('#btnEdit').attr("disabled", "disabled");
        $('#btnDelete').attr("disabled", "disabled");
    }
})();
