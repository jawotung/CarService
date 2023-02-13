"use strict";
(function () {
    var ajax = $D();
    var tblWalkIn = "";
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

        $('#tblWalkIn tbody').on('click', 'tr', function (e) {
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
                    var data = tblWalkIn.row($(this)).data();
                    if ($.trim(data) != "") {
                        if ($(this).hasClass('selected')) {
                            Edit();
                        }
                        else {
                            tblWalkIn.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            $('#btnEdit').removeAttr("disabled");
                            $('#btnDelete').removeAttr("disabled");
                        }
                    }
                    break;
            }
        });
        $("#tblWalkIn").on("change", '.columnSearch', function () {
            tblWalkIn.ajax.reload(null, false);
        });

        $("#btnAdd").click(function () {
            $("#mdlWalkIn").modal("show");
            tblWalkIn.ajax.reload(null, false);
            cancelTbl();
            cancelForm();
        });
        $('#btnEdit').click(function () {
            Edit();
        });
        $('#btnDelete').click(function () {
            var data = tblWalkIn.rows('.selected').data()[0];
            ajax.msg = "Are you sure you want to delete this data?";
            ajax.confirmAction().then(function (approve) {
                if (approve) {
                    ajax.formAction = '/MasterMaintenance/WalkInMaster/DeleteWalkIn';
                    ajax.jsonData = { ID: data.ID };
                    ajax.sendData().then(function () {
                        tblWalkIn.ajax.reload(null, false);
                        cancelTbl();
                        cancelForm();
                    });
                }
            });
        });
        $("#frmWalkIn").submit(function (e) {
            e.preventDefault();
            ajax.formData = $('#frmWalkIn').serializeArray();
            ajax.formAction = '/MasterMaintenance/WalkInMaster/SaveWalkIn';
            ajax.setJsonData().sendData().then(function () {
                tblWalkIn.ajax.reload(false);
                cancelTbl();
                cancelForm();
            });
        });
        $("#FirstName, #MiddleName, #LastName").blur(function () {
            $(this).val($Helper().MakeFirstLetterUpper($(this).val()));
        });
    });

    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblWalkIn')) {
            tblWalkIn = $('#tblWalkIn').DataTable({
                searching: false,
                "pageLength": 25,
                "ajax": {
                    "url": "/MasterMaintenance/WalkInMaster/GetWalkInList",
                    "type": "GET",
                    "datatype": "json",
                },
                dataSrc: "data",
                select: true,
                columns: [
                    { title: "WalkInID", data: "WalkInID" },
                    { title: "FullName", data: 'FullName' },
                    { title: "Position", data: 'PositionName' },
                ],
            })
        }
    }
    function Edit() {
        var data = tblWalkIn.rows('.selected').data()[0];
        $("#frmWalkIn").parsley().reset();
        $("#mdlWalkInTitle").text(" Update WalkIn");
        $('#WalkInID').prop('readonly', true);
        $("#btnSave .btnLabel").text(" Update");
        ajax.populateToFormInputs(data, "#frmWalkIn");
        $("#Password").val("");
        $("#Password").attr('required', false);
        $("#mdlWalkIn").modal("show");
    }
    function cancelForm() {
        ajax.clearFromData("frmWalkIn");
        $('#WalkInID').prop('readonly', false);
        $("#mdlWalkInTitle").text(" Create WalkIn");
        $("#btnSave .btnLabel").text(" Save");
        $("#Password").val("");
        $("#Password").attr('required', true);
        $("#mdlWalkIn").modal("hide");
    }
    function cancelTbl() {
        $('#btnEdit').attr("disabled", "disabled");
        $('#btnDelete').attr("disabled", "disabled");
    }
})();
