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

Number.prototype.round = function (places) {
    return +(Math.round(this + "e+" + places) + "e-" + places);
}

function OnAddRow() {
    var rowCount = $('.exceldatarow').length+1;
    var row = '<div class="primary row exceldatarow">';
    if (rowCount % 2 == 0)
        row = '<div class="secondary row exceldatarow">';
   row = row + '<div class="col-4 row"> '+
        '<div class="col-1">' + rowCount + '</div>'+
    '<div class="col-2 text-align-left">W/D</div>'+
    '<div class="col-5 text-align-left">   Item Name </div>'+

    '<div class="col-4 text-align-left"> Detail #/Page </div>'+
        '</div>'+
        ' <div class="col text-align-left">Takeoff Color</div>' +
    '<div class="col-5 row text-align-left">' +
    '    <div class="col text-align-left">Length</div>' +
    '   <div class="col text-align-left">Width</div>' +
    '<div class="col text-align-left">Height</div>' +
    '<div class="col text-align-left">Pcs</div>' +
    '<div class="col text-align-left"> Mold QTY </div>' +
    '</div>' +
    '<div class="col-2 row light-green"></div>' +
    '<div class="col-4 row">' +
    '<div class="col-1">&nbsp;</div>' +
    '<div class="col-2">' +
    '<input id="row' + rowCount + 'projectDetailId" name="row' + rowCount + 'projectDetailId" type="hidden"  />' +
    '<select id="row' + rowCount + 'WD" name="row' + rowCount + 'WD" class="form-select" style="width:60px;">' +
    '<option value="Dry">Dry</option>' +
    '<option value="Wet">Wet</option>' +
    '</select>' +
    '</div>' +
    '<div class="col-5">' +
        '<input id="row' + rowCount + 'ItemName" name="row' + rowCount + 'ItemName" class="form-control" type="text" />' +
    '</div>' +

    '<div class="col-4">' +
    '<input id="row' + rowCount + 'DetailPage" name="row' + rowCount + 'DetailPage" class="form-control" type="text" />' +
        '</div>' +
    '<div class="col-12">' +
    '<div class="col-3 float-left">&nbsp;</div>' +
    '<div class="col-9  float-left text-align-left">' +

    '<label for="inputPassword" class="col-7 "> Disposition (Special Notes)</label>' +


    '<textarea id="row' + rowCount + 'DispositionSpecialNote" name="row' + rowCount + 'DispositionSpecialNote" style="height:80px;" class="form-control" type="text" ></textarea>' +

    '</div>' +
        '</div>' +
    '</div>' +
    '<div class="col-1">' +
    '<div class="col">' +
    '<input id="row' + rowCount + 'TakeOffColor" name="row' + rowCount + 'TakeOffColor" type="text" class="form-control" style="width:100px"  />' +
        '</div>' +
    '<div class="col text-align-left"> Section</div>' +
    '<div class="col-12 no-margin text-align-left">' +
                                       
    '</div>' +
    '<div class="col-12 text-align-left">' +
    '<input type="file" id="row' + rowCount + 'File" name="row' + rowCount + 'File" class="form-control" />' +
        '</div>' +

    '</div>' +
    '<div class="col-5 row">' +

    '<div class="col">' +
    '<input id="row' + rowCount + 'Length" name="row' + rowCount + 'Length" type="text" onkeypress="return isNumberKey(this, event);" class="form-control" onchange="CalculateActCFPcs(' + rowCount + ');CalculateNomCFPcs(' + rowCount + ');" style="width:70px" />' +
        '</div>' +
    '<div class="col">' +
    '<input id="row' + rowCount + 'Width" name="row' + rowCount + 'Width" type="text" onkeypress="return isNumberKey(this, event);" onchange="CalculateNomCFLF(' + rowCount + ');" class="form-control" style="width:70px" />' +
        '</div>' +
    '<div class="col">' +
    '<input id="row' + rowCount + 'Height" name="row' + rowCount + 'Height" type="text" onkeypress="return isNumberKey(this, event);" onchange="CalculateNomCFLF(' + rowCount + ');" class="form-control" style="width:70px" />' +
        '</div>' +
    '<div class="col">' +
    '<input id="row' + rowCount + 'Pieces" name="row' + rowCount + 'Pieces" type="text" onkeypress="return isNumberKey(this, event);" class="form-control" style="width:70px" />' +
    '</div>' +




    '<div class="col">' +
    '<input id="row' + rowCount + 'MoldQTY" name="row' + rowCount + 'MoldQTY" type="text" onkeypress="return isNumberKey(this, event);" class="form-control" style="width:70px" />' +
    '</div>' +
    '<div class="row">' +
    '<div class="col text-align-left">Total LF</div>' +
    '<div class="col text-align-left"></div>' +
    '<div class="col text-align-left">Nom. CF/LF</div>' +
    '<div class="col text-align-left">Total Nom. CF</div>' +
    '<div class="col text-align-left"></div>' +
    '</div>' +
    '<div class="row">' +
    '<div class="col">' +
    '<input id="row' + rowCount + 'TotalLF" name="row' + rowCount + 'TotalLF" type="text" onkeypress="return isNumberKey(this, event);" onchange="CalculateTotalActCF(' + rowCount + ');CalculateTotalNomCF(' + rowCount + ');CalculateActSFCFLF(' + rowCount + ');" class="form-control" style="width:70px"  />' +
    '</div>' +
    '<div class="col"></div>' +
    '<div class="col float-left">' +
    '<input id="row' + rowCount + 'NomCFLF" name="row' + rowCount + 'NomCFLF" type="text" onkeypress="return isNumberKey(this, event);" class="form-control" onchange="CalculateNomCFPcs(' + rowCount + ');CalculateTotalNomCF(' + rowCount + ');" style="width:70px"  />' +
    '</div>' +
    '<div class="col float-left">' +
    '<input id="row' + rowCount + 'TotalNomCF" name="row' + rowCount + 'TotalNomCF" readonly="readonly" type="text" class="form-control" style="width:70px" onchange="CalculateTotalNominal(' + rowCount + ')"  />' +
    '</div>' +
    '<div class="col"></div>' +
        '</div>' +
    '<div class="row">' +
    '<div class="col text-align-left">Act SF CF/LF</div>' +
    '<div class="col text-align-left">Act CF/Pcs</div>' +
    '<div class="col text-align-left">Total Act CF</div>' +
    '<div class="col text-align-left">Nom. CF/Pcs</div>' +
    '<div class="col"></div>' +
    '</div>' +
    '<div class="row">' +
    '<div class="col float-left">' +
    '<input id="row' + rowCount + 'ActSFCFLF" name="row' + rowCount + 'ActSFCFLF" type="text" readonly="readonly" onkeypress="return isNumberKey(this, event);" class="form-control" onchange="CalculateActCFPcs(' + rowCount + ');CalculateTotalActCF(' + rowCount + ');" style="width:70px"  />' +
        '</div>' +
    '<div class="col float-left">' +
    '<input id="row' + rowCount + 'ActCFPcs" name="row' + rowCount + 'ActCFPcs" type="text" onkeypress="return isNumberKey(this, event);" readonly="readonly" class="form-control" style="width:70px"  />' +
        '</div>' +
    '<div class="col float-left">' +
    '<input id="row' + rowCount + 'TotalActCF" name="row' + rowCount + 'TotalActCF" readonly="readonly" type="text" onchange="CalculateTotalActual(' + rowCount + ');" class="form-control" style="width:70px"  />' +
    '</div>' +

    '<div class="col float-left">' +
    '<input id="row' + rowCount + 'NomCFPcs" name="row' + rowCount + 'NomCFPcs" readonly="readonly" type="text" class="form-control" style="width:70px"  />' +
    '</div>' +
    '<div class="col"></div>' +

    '</div>' +
    '</div>' +
    '<div class="col-2 row">' +
    '<div class="light-green col float-left">' +
    '<span class="float-left">$</span><input id="row' + rowCount + 'TotalActual" name="row' + rowCount + 'TotalActual" readonly="readonly" type="text" class="form-control" style="width:70px;background-color:transparent !important; border: 0px solid;" />' +
    '</div>' +
        '<div class="light-green col float-left">' +
        '<span class="float-left">$</span><input id="row' + rowCount + 'TotalNominal" name="row' + rowCount + 'TotalNominal" readonly="readonly" type="text" class="form-control" style="width:70px;background-color:transparent !important; border: 0px solid;" />' +
    '</div>' +
                                '</div>'+
                            '</div > ';
    $('#RowsTable').append(row);
}

function CalculateTotalActCF(rownumber) {
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var ActSFCFLF = document.getElementById('row' + rownumber + 'ActSFCFLF').value;

    if (ActSFCFLF !== "" && TotalLF !== "") {
        document.getElementById('row' + rownumber + 'TotalActCF').value = ((ActSFCFLF / 12) * TotalLF).round(2);
        document.getElementById('rowFP' + rownumber + 'TotalActCF').value = ((ActSFCFLF / 12) * TotalLF).round(2);        
    }
    else
        document.getElementById('row' + rownumber + 'TotalActCF').value = "";
    CalculateTotalActual(rownumber);
}

function CalculateNomCFLF(rownumber) {
    var Width = document.getElementById('row' + rownumber + 'Width').value;
    var Height = document.getElementById('row' + rownumber + 'Height').value;

    if (Width !== "" && Height !== "") {
        document.getElementById('row' + rownumber + 'NomCFLF').value = ((Width * Height) / 144).round(2);
        document.getElementById('rowFP' + rownumber + 'NomCFLF').value = ((Width * Height) / 144).round(2);
    }
    else
        document.getElementById('row' + rownumber + 'NomCFLF').value = "";
    CalculateTotalNomCF(rownumber);
}

function CalculateNomCFPcs(rownumber) {
    var Length = document.getElementById('row' + rownumber + 'Length').value;
    var NomCFLF = document.getElementById('row' + rownumber + 'NomCFLF').value;

    if (Length !== "" && NomCFLF !== "") {
        document.getElementById('row' + rownumber + 'NomCFPcs').value = (Length * NomCFLF).round(2);
        document.getElementById('rowFP' + rownumber + 'NomCFPcs').value = (Length * NomCFLF).round(2);
    }
    else
        document.getElementById('row' + rownumber + 'NomCFPcs').value = "";
}

function CalculateTotalNomCF(rownumber) {
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var NomCFLF = document.getElementById('row' + rownumber + 'NomCFLF').value;

    if (TotalLF !== "" && NomCFLF !== "") {
        document.getElementById('row' + rownumber + 'TotalNomCF').value = (TotalLF * NomCFLF).round(2);
        document.getElementById('rowFP' + rownumber + 'TotalNomCF').value = (TotalLF * NomCFLF).round(2);
        CalculateTotalNominal(rownumber);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalNomCF').value = "";
        document.getElementById('rowFP' + rownumber + 'TotalNomCF').value = "";
    }
}

function CalculateActSFCFLF(rownumber) {
    var Width = document.getElementById('row' + rownumber + 'Width').value;
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var Length = document.getElementById('row' + rownumber + 'Length').value;

    if (TotalLF !== "" && Width !== "" && Length !== "") {
        document.getElementById('row' + rownumber + 'ActSFCFLF').value = (Length * Width / 144 * TotalLF).round(2);
    }
    else
        document.getElementById('row' + rownumber + 'ActSFCFLF').value = "";
    CalculateActCFPcs(rownumber);
    CalculateTotalActCF(rownumber);
}

function CalculateActCFPcs(rownumber) {
    var length = document.getElementById('row' + rownumber + 'Length').value;
    var ActSFCFLF = document.getElementById('row' + rownumber + 'ActSFCFLF').value;

    if (ActSFCFLF !== "" && length !== "") {
        document.getElementById('row' + rownumber + 'ActCFPcs').value = ((ActSFCFLF / 12) * length).round(2);
    }
    else
        document.getElementById('row' + rownumber + 'ActCFPcs').value = "";
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

function CalculateTotalActual(rownumber) {
    var TotalActCF = document.getElementById('row' + rownumber + 'TotalActCF').value;
    var ActualCF = document.getElementById('ActualCF').value;

    if (TotalActCF !== "" && ActualCF !== "") {
        document.getElementById('row' + rownumber + 'TotalActual').value = (TotalActCF * ActualCF).round(2);
        document.getElementById('rowFP' + rownumber + 'TotalActual').value = (TotalActCF * ActualCF).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalActual').value = "";
        document.getElementById('rowFP' + rownumber + 'TotalActual').value = "";
    }
}

function CalculateTotalActualOnChangeOfActualCF() {
    
    var ActualCF = document.getElementById('ActualCF').value;
    var rowCount = $('.exceldatarow').length;

    for (var rownumber = 1; rownumber <= rowCount; rownumber++) { 
        var TotalActCF = document.getElementById('row' + rownumber + 'TotalActCF').value;
        if (TotalActCF !== "" && ActualCF !== "") {
            document.getElementById('row' + rownumber + 'TotalActual').value = (TotalActCF * ActualCF).round(2);
        }
        else
                document.getElementById('row' + rownumber + 'TotalActual').value = "";
    }
}

function CalculateTotalNominal(rownumber) {
    var TotalNomCF = document.getElementById('row' + rownumber + 'TotalNomCF').value;
    var NominalCF = document.getElementById('NominalCF').value;

    if (TotalNomCF !== "" && NominalCF !== "") {
        document.getElementById('row' + rownumber + 'TotalNominal').value = (TotalNomCF * NominalCF).round(2);
        document.getElementById('rowFP' + rownumber + 'TotalNominal').value = (TotalNomCF * NominalCF).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalNominal').value = "";
        document.getElementById('rowFP' + rownumber + 'TotalNominal').value = "";
    }
}

function CalculateTotalNominalOnChangeOfNominalCF() {

    var NominalCF = document.getElementById('NominalCF').value;
    var rowCount = $('.exceldatarow').length;

    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalNomCF = document.getElementById('row' + rownumber + 'TotalNomCF').value;
        if (TotalNomCF !== "" && NominalCF !== "") {
            document.getElementById('row' + rownumber + 'TotalNominal').value = (TotalNomCF * NominalCF).round(2);
        }
        else
            document.getElementById('row' + rownumber + 'TotalNominal').value = "";
    }
}

function ShowImageSelection(rowNumber) {

    document.getElementById('row' + rowNumber + 'File').click();
}

function SumTotalOfActual() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalActual = document.getElementById('rowFP' + rownumber + 'TotalActual').value;
        if (TotalActual != "")
            sum = sum + parseFloat( TotalActual);
    }
    document.getElementById('rowFPSumTotalActual').value = sum;
}

function SumTotalOfNominal() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalNominal = document.getElementById('rowFP' + rownumber + 'TotalNominal').value;
        if (TotalNominal != "")
            sum = sum + parseFloat(TotalNominal);
    }
    document.getElementById('rowFPSumTotalNominal').value = sum;
}

function OnLineItemChange(rownumber) {
    var LineItemCharge = document.getElementById('rowFP' + rownumber + 'LineItemCharge').value;
    var TotalCheckBox = $("input[name='rowFP" + rownumber + "TotalCheckBox']:checked").val(); 
    var totalValue = 0;
    if (TotalCheckBox == "1") {
        totalValue = document.getElementById('rowFP' + rownumber + 'TotalActual').value;
    }
    else
        totalValue = document.getElementById('rowFP' + rownumber + 'TotalNominal').value;

    if (LineItemCharge != "" && totalValue != "") {
        document.getElementById('rowFP' + rownumber + 'LineData').value = (LineItemCharge - totalValue).round(2);
    }
}

function OnLineItemChange(rownumber) {
    var LineItemCharge = document.getElementById('rowFP' + rownumber + 'LineItemCharge').value;
    var TotalCheckBox = $("input[name='rowFP" + rownumber + "TotalCheckBox']:checked").val();
    var TotalNomCF = document.getElementById('rowFP' + rownumber + 'TotalNomCF').value;
    var TotalActCF = document.getElementById('rowFP' + rownumber + 'TotalActCF').value;
    var totalValue = 0;
    if (TotalCheckBox == "1") {
        totalValue = document.getElementById('rowFP' + rownumber + 'TotalActual').value;
    }
    else
        totalValue = document.getElementById('rowFP' + rownumber + 'TotalNominal').value;

    if (LineItemCharge != "" && totalValue != "") {
        document.getElementById('rowFP' + rownumber + 'LineData').value = (LineItemCharge - totalValue).round(2);
    }

    if (LineItemCharge != "" && TotalNomCF != "") {
        document.getElementById('rowFP' + rownumber + 'LineItemChargePCs').value = (LineItemCharge / TotalNomCF).round(2);
    }

    if (LineItemCharge != "" && TotalNomCF != "") {
        document.getElementById('rowFP' + rownumber + 'LineItemChargeCF').value = (LineItemCharge / TotalActCF).round(2);
    }
}