// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function Validate() {
    if (document.getElementById('projectname').value === "") {
        alert("Enter project name");
        return false;
    }
    return true;
}

function OnAddRow() {
    var rowCount = $('#RowsTable tr').length;
    var row = '<tr>' +
        '<td>' +
        '<select id="row' + rowCount + 'WD" name="row' + rowCount + 'WD"  class="form-control" >' +
        '<option value="Dry">Dry</option>' +
        '<option value="Wet">Wet</option>' +
        '</select>' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'ItemName" name="row' + rowCount + 'ItemName" type="text"  class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'DispositionSpecialNote" name="row' + rowCount + 'DispositionSpecialNote" type="text" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'DetailPage" name="row' + rowCount + 'DetailPage" type="text" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input type="file" id="row' + rowCount + 'File" name="row' + rowCount + 'File" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'TakeOffColor" name="row' + rowCount + 'TakeOffColor" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'Length" name="row' + rowCount + 'Length" onkeypress="return isNumberKey(this, event);" type="text" onchange="CalculateActCFPcs(' + rowCount + ');CalculateNomCFPcs(' + rowCount + ');" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'Width" name="row' + rowCount + 'Width" onkeypress="return isNumberKey(this, event);" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'Height" name="row' + rowCount + 'Height" onkeypress="return isNumberKey(this, event);" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'Pieces" name="row' + rowCount + 'Pieces" onkeypress="return isNumberKey(this, event);" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'TotalLF" name="row' + rowCount + 'TotalLF" onkeypress="return isNumberKey(this, event);" onchange="CalculateTotalActCF(' + rowCount + ');CalculateTotalNomCF(' + rowCount + ');" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'ActSFCFLF" name="row' + rowCount + 'ActSFCFLF" onkeypress="return isNumberKey(this, event);" onchange="CalculateActCFPcs(' + rowCount + ');CalculateTotalActCF(' + rowCount + ');" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'ActCFPcs" name="row' + rowCount + 'ActCFPcs" readonly="readonly" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'TotalActCF" name="row' + rowCount + 'TotalActCF" readonly="readonly" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'NomCFLF" name="row' + rowCount + 'NomCFLF" type="text" onkeypress="return isNumberKey(this, event);" onchange="CalculateNomCFPcs(' + rowCount + ');CalculateTotalNomCF(' + rowCount + ');" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'NomCFPcs" name="row' + rowCount + 'NomCFPcs" readonly="readonly" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'TotalNomCF" name="row' + rowCount + 'TotalNomCF" readonly="readonly" type="text" style="width:50px" class="form-control"  />' +
        '</td>' +
        '<td>' +
        '<input id="row' + rowCount + 'MoldQTY" name="row' + rowCount + 'MoldQTY" type="text" onkeypress="return isNumberKey(this, event);" style="width:50px" class="form-control"  />' +
        '</td>' +
        '</tr>';
    $('#RowsTable').append(row);
}

function CalculateActCFPcs(rownumber) {
    var length = document.getElementById('row' + rownumber + 'Length').value;
    var ActSFCFLF = document.getElementById('row' + rownumber + 'ActSFCFLF').value;

    if (ActSFCFLF !== "" && length !== "") {
        document.getElementById('row' + rownumber + 'ActCFPcs').value = (ActSFCFLF / 12) * length;
    }
    else
        document.getElementById('row' + rownumber + 'ActCFPcs').value = "";
}

function CalculateTotalActCF(rownumber) {
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var ActSFCFLF = document.getElementById('row' + rownumber + 'ActSFCFLF').value;

    if (ActSFCFLF !== "" && TotalLF !== "") {
        document.getElementById('row' + rownumber + 'TotalActCF').value = (ActSFCFLF / 12) * TotalLF;
    }
    else
        document.getElementById('row' + rownumber + 'TotalActCF').value = "";
}

function CalculateNomCFLF(rownumber) {
    var Width = document.getElementById('row' + rownumber + 'Width').value;
    var Height = document.getElementById('row' + rownumber + 'Height').value;

    if (Width !== "" && Height !== "") {
        document.getElementById('row' + rownumber + 'NomCFLF').value = Width * Height;
    }
    else
        document.getElementById('row' + rownumber + 'NomCFLF').value = "";
}

function CalculateNomCFPcs(rownumber) {
    var Length = document.getElementById('row' + rownumber + 'Length').value;
    var NomCFLF = document.getElementById('row' + rownumber + 'NomCFLF').value;

    if (Length !== "" && NomCFLF !== "") {
        document.getElementById('row' + rownumber + 'NomCFPcs').value = Length * NomCFLF;
    }
    else
        document.getElementById('row' + rownumber + 'NomCFPcs').value = "";
}

function CalculateTotalNomCF(rownumber) {
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var NomCFLF = document.getElementById('row' + rownumber + 'NomCFLF').value;

    if (TotalLF !== "" && NomCFLF !== "") {
        document.getElementById('row' + rownumber + 'TotalNomCF').value = TotalLF * NomCFLF;
    }
    else
        document.getElementById('row' + rownumber + 'TotalNomCF').value = "";
}

function isNumberKey(txt, evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
            return true;
        } else {
            return false;
        }
    } else {
        if (charCode > 31 &&
            (charCode < 48 || charCode > 57))
            return false;
    }
    return true;
}