"use strict";
(function () {
    var General = $D();
    var CUI2 = $UI();
    var tblGeneral = "";
    var tblType = "";
    var editIDGeneral = "";
    var editIDType = "";
    var addMode = false;
    var UpdateDateGeneral = "";
    var UpdateDateType = "";
    $(document).ready(function() {
        drawDatatables();
        GetCommon();
        //General Events Here
        $('#btnNewGeneral').click(function () {
            $("#mdlGeneral").modal("show");
            $('#TypeID').prop("disabled", false);
            cancelGeneralTbl();
            cancelGeneralForm();
            tblGeneral.ajax.reload(false);
        });
        $("#frmGeneral").submit(function (e) {
            e.preventDefault();
            saveGeneral();
        });
        $('#tblGeneral tbody').on('click', 'tr', function () {
            dis_se_lectGeneralRow($(this));
        });
        $('#btnEditGeneral').click(function () {
            editGeneral();
        });
        $('#btnDeleteGeneral').click(function () {
            General.msg = "Are you sure you want to delete this data?";
            General.confirmAction().then(function (approve) {
                if (approve)
                    deleteGeneral();
            });
        });
        $("#SearchType").change(function () {
            tblGeneral.ajax.reload(false);
        });
        $(".onChangeParsley").change(function (e) {
            General.parsleyValidate("frmGeneral");
        })

        //Type Events here
        $('#btnNewType').click(function () {
            $("#mdlType").modal("show");
            $("#Type").focus();
            cancelTypeTbl();
            cancelTypeForm();
            tblType.ajax.reload(null, false);
        });
        $("#Type").change(function () {
            validateType();
        });
        $("#frmType").submit(function (e) {
            e.preventDefault();
            saveType();
        });
        $('#tblType tbody').on('click', 'tr', function () {
            dis_se_lectTypeRow($(this));
            cancelGeneralTbl();
            cancelGeneralForm();
        });
        $('#btnEditType').click(function () {
            editType();
        });
        $('#btnDeleteType').click(function () {
            General.msg = "Are you sure you want to delete this data?";
            General.confirmAction().then(function (approve) {
                if (approve)
                    deleteType();
            });
        });
        //#Special Events
        tblGeneral.on('draw.dt', function () {
            CUI.dataTableID = "#tblGeneral";
            CUI.setDatatableMaxHeight();
        });
        tblType.on('draw.dt', function () {
            CUI2.dataTableID = "#tblType";
            CUI2.setDatatableMaxHeight();
        });
        $.listen('parsley:field:error', function (fieldInstance) {//Parsley Validation Error Event Listener
            setTimeout(function () { CUI.setDatatableMaxHeight(); }, 500);
        });

        $('#btnPrintField').on('click', function (e) {
            window.location = "/MasterMaintenance/GeneralMaster/PrintField?TypeID=" + editIDType;
        });
        //Library Inits
        $('#mdlGeneral').on('shown.bs.modal', function (e) {
            $('#TypeID').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        return {
                            q: params.term,
                            id: 'ID',
                            text: 'Type',
                            table: 'mTypes',
                            db: 'CarService',
                            condition: '',
                            display: 'id&text',
                            orderBy: ' ORDER BY Type '
                        };
                    },
                },
                placeholder: '--Please Select--',
                sorter: data => data.sort((a, b) => a.text.localeCompare(b.text)),
            })
            if (addMode) {
                var rowData = tblType.rows({ selected: true }).data()[0];
                var typeOption = new Option(rowData.Type, rowData.ID, true, true);
                $('#TypeID').append(typeOption).trigger('change');
            }
        })


        $('.btnCommon').on('click', function (e) {
            var Name = $(this).attr("data-Name");
            var Value = $("#" + Name).val();
            General.formAction = '/MasterMaintenance/GeneralMaster/UpdateCommon';
            General.jsonData = { Name: Name, Value: Value}
            General.sendData().then(function () {
                GetCommon();
            });
        });
    });
    function drawDatatables() {
        if (!$.fn.DataTable.isDataTable('#tblGeneral')) {
            tblGeneral = $('#tblGeneral').DataTable({
                processing: true,
                serverSide: true,
                "order": [[0, "asc"]],
                "pageLength": 25,
                "ajax": {
                    "url": "/MasterMaintenance/GeneralMaster/GetGeneralList",
                    "type": "POST",
                    "datatype": "json",
                    "data": function (d) {
                        d.TypeID = editIDType;
                    }
                },
                responsive: true,
                dataSrc: "data",
                columns: [
                    { title: "Type", data: "TypeDesc" },
                    { title: "Value", data: "Value" },
                ],
                "createdRow": function (row, data, dataIndex) {
                    $(row).attr('data-id', data.ID);
                    $(row).attr('data-UpdateDate', data.UpdateDate);
                }
            })
        }
        if (!$.fn.DataTable.isDataTable('#tblType')) {
            tblType = $('#tblType').DataTable({
                processing: true,
                serverSide: true,
                "order": [[0, "asc"]],
                "pageLength": 25,
                "ajax": {
                    "url": "/MasterMaintenance/GeneralMaster/GetTypeList",
                    "type": "POST",
                    "datatype": "json",
                },
                responsive: true,
                dataSrc: "data",
                columns: [
                    { title: "Type", data: "Type" },
                ],
                "createdRow": function (row, data, dataIndex) {
                    $(row).attr('data-id', data.ID);
                    $(row).attr('data-UpdateDate', data.UpdateDate);
                }
            })
        }
    }
    function GetCommon() {
        General.formAction = '/MasterMaintenance/GeneralMaster/GetCommon';
        General.jsonData = {}
        General.sendData().then(function () {
            $("#Leftside").val(General.responseData.Leftside);
            $("#Rightside").val(General.responseData.Rightside);
        });
    }
    function validateValue() {
        var type = $("#TypeID").val().trim();
        var value = $("#Value").val().trim();
        if (type && type) {
            General.formAction = '/MasterMaintenance/GeneralMaster/ValidateValue';
            General.jsonData = { TypeID: type, Value: value }
            General.sendData().then(function () {
                var validValue = General.responseData.isValid;
                if (!validValue) {
                    General.showError("Type and Value comibination already exists. Please try different value.");
                    $("#Value").val("");
                    $("#Value").focus();
                }
            });
        }
    }
    function saveGeneral() {
        $('#TypeID').prop("disabled", false);
        General.formData = $('#frmGeneral').serializeArray();
        General.formAction = '/MasterMaintenance/GeneralMaster/SaveGeneral';
        General.setJsonData().sendData().then(function () {
            tblGeneral.ajax.reload(null, false);
            cancelGeneralTbl();
            cancelGeneralForm();
        });
    }
    function cancelGeneralForm() {
        General.clearFromData("frmGeneral");
        $('#TypeID').prop('readonly', false);
        $("#btnSaveGeneral .btnLabel").text(" Save");
        $("#mdlGeneralTitle").text(" Create Value");
        addMode = false;
        $("#mdlGeneral").modal("hide");
        editIDGeneral = "";
        UpdateDateGeneral = "";
    }
    function cancelGeneralTbl() {
        $('#btnEditGeneral').attr("disabled", "disabled");
        $('#btnDeleteGeneral').attr("disabled", "disabled");
    }
    function dis_se_lectGeneralRow(GeneralRow) {
        if (GeneralRow.data('id')) {
            if (GeneralRow.hasClass('selected')) {
                GeneralRow.removeClass('selected');
                editIDGeneral = "";
                UpdateDateGeneral = "";
                $('#btnEditGeneral').attr("disabled", "disabled");
                $('#btnDeleteGeneral').attr("disabled", "disabled");
            }
            else {
                tblGeneral.$('tr.selected').removeClass('selected');
                GeneralRow.addClass('selected');
                editIDGeneral = GeneralRow.data("id");
                UpdateDateGeneral = GeneralRow.data("updatedate");
                $('#btnEditGeneral').removeAttr("disabled");
                $('#btnDeleteGeneral').removeAttr("disabled");
            }
        }
    }
    function editGeneral() {
        General.formAction = '/MasterMaintenance/GeneralMaster/GetGeneralDetails';
        if (General.formAction) {
            General.jsonData = { ID: editIDGeneral };
            General.sendData().then(function () {
                populateGeneralData(General.responseData.generalData);
            });
        } else {
            General.showError("Please try again.");
        }
    }
    function populateGeneralData(general) {
        //$('#TypeID').prop('readonly', true);
        $("#frmGeneral").parsley().reset();
        $("#btnSaveGeneral .btnLabel").text(" Update");
        $("#mdlGeneralTitle").text(" Update Value");
        General.populateToFormInputs(general, "#frmGeneral");

        var typeOption = new Option(general.TypeDesc, general.TypeID, true, true);
        $('#TypeID').append(typeOption).trigger('change');
        $('#TypeID').prop("disabled", true);

        $("#mdlGeneral").modal("show");
    }
    function deleteGeneral() {
        General.formAction = '/MasterMaintenance/GeneralMaster/DeleteGeneral';
        General.jsonData = { ID: editIDGeneral, UpdateDate: UpdateDateGeneral };
        General.sendData().then(function () {
            tblGeneral.ajax.reload(null, false);
            cancelGeneralTbl();
            cancelGeneralForm();
        });
    }

    //Type Functions Here
    function validateType() {
        var type = $("#Type").val().trim();
        if (type) {
            General.formAction = '/MasterMaintenance/GeneralMaster/ValidateType';
            General.jsonData = { Type: type }
            General.sendData().then(function () {
                var validValue = General.responseData.isValid;
                if (!validValue) {
                    General.showError("Type already exists. Please try again.");
                    $("#Type").val("");
                    $("#Type").focus();
                }
            });
        }
    }
    function cancelTypeForm() {
        General.clearFromData("frmType");
        $('#TypeMainID').prop('readonly', false);
        $("#btnSaveType .btnLabel").text(" Save");
        $("#mdlTypeTitle").text(" Create Type");
        $("#mdlType").modal("hide");
        editIDType = '';
    }
    function saveType() {
        General.formData = $('#frmType').serializeArray();
        General.formAction = '/MasterMaintenance/GeneralMaster/SaveType';
        General.setJsonData().sendData().then(function () {
            cancelTypeTbl();
            cancelTypeForm();
            tblType.ajax.reload(null, false);
        });
    }
    function cancelTypeTbl() {
        $('#btnEditType').attr("disabled", "disabled");
        $('#btnDeleteType').attr("disabled", "disabled");
    }
    function dis_se_lectTypeRow(TypeRow) {
        if (TypeRow.hasClass('selected')) {
            TypeRow.removeClass('selected');
            editIDType = "";
            UpdateDateType = "";
            addMode = false;
            $('#btnEditType').attr("disabled", "disabled");
            $('#btnDeleteType').attr("disabled", "disabled");
        }
        else {
            tblType.$('tr.selected').removeClass('selected');
            TypeRow.addClass('selected');
            editIDType = TypeRow.data("id");
            UpdateDateType = TypeRow.data("updatedate");
            addMode = true;
            $('#btnEditType').removeAttr("disabled");
            $('#btnDeleteType').removeAttr("disabled");
        }
        tblGeneral.ajax.reload(null, false);
    }
    function editType() {
        General.formAction = '/MasterMaintenance/GeneralMaster/GetTypeDetails';
        if (General.formAction) {
            General.jsonData = { ID: editIDType };
            General.sendData().then(function () {
                populateTypeData(General.responseData.generalData);
            });
        } else {
            General.showError("Please try again.");
        }
    }
    function populateTypeData(type) {
        //$('#TypeID').prop('readonly', true);
        $("#frmType").parsley().reset();
        $("#btnSaveType .btnLabel").text(" Update");
        $("#mdlTypeTitle").text(" Update Type");
        General.populateToFormInputs(type, "#frmType");
        $("#mdlType").modal("show");
    }
    function deleteType() {
        General.formAction = '/MasterMaintenance/GeneralMaster/DeleteType';
        General.jsonData = { ID: editIDType, UpdateDate: UpdateDateType };
        General.sendData().then(function () {
            tblType.ajax.reload(null, false);
            tblGeneral.ajax.reload(null, false);
            cancelTypeTbl();
            cancelTypeForm();
        });
    }
})();
