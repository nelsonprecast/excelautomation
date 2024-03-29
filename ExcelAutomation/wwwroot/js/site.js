﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

function allowDropDiv(ev) {
    ev.preventDefault();
}

function dragDiv(ev) {
    ev.dataTransfer.setData("projectDetailId", ev.target.id);
}

function dropDiv(ev) {
    ev.preventDefault();
    var projectDetailId = ev.dataTransfer.getData("projectDetailId");
    var newGroupId = ev.target.id;
    
    $.ajax({
        url: '/Group/ChangeGroup/?projectDetailId=' + projectDetailId + '&GroupId=' + newGroupId,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (response) {
            window.location.reload();
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function OnAddRow() {
    var rowCount = $('.exceldatarow').length + 1;
    $.ajax({
        url: '/Home/GetProjectDetailView/?id=' + rowCount,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (response) {
            $('#RowsTable').append(response);
            BindEvent();
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });  
}

// Get the button that opens the modal
var newGroupBtn = document.getElementById("newGroupBtn");
var closeGroupModel = document.getElementById("closeGroupModel");
// Get the modal
var modal = document.getElementById("groupModal");

function DoConvertToPdf(projectId) {

    var id = document.getElementById('convertToPdfDownload');
    id.href = '/Home/ConvertToPdf/?projectId=' + projectId;
    id.click();

    
}

function OnCreateGroup() {
    modal.style.display = "none";
    var projectDetails = [];
    $("input[type = 'checkbox']").each(function () {       
        var c = $(this).is(":checked")
        if (c) {
            var v = $(this).val();
            projectDetails.push(parseInt(v));
        }
    });
    var gName = $('#groupName1').val();
    var postData = {
        GroupId: null,
        GroupName: gName,
        ProjectDetailIds: projectDetails
    }
    $.ajax({
        type: 'POST',
        url: "/Group/CreateGroup",
        'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: postData,
        success: function (resultData) {
            window.location.reload();
        }
    });  
}


// Get the <span> element that closes the modal


// When the user clicks the button, open the modal 
newGroupBtn.onclick = function () {
    modal.style.display = "block";
}

closeGroupModel.onclick = function () {
    modal.style.display = "none";
}



// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
function CalculateTotalActCF(rownumber) {
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var ActSFCFLF = document.getElementById('row' + rownumber + 'ActSFCFLF').value;

    if (ActSFCFLF !== "" && TotalLF !== "") {
        document.getElementById('row' + rownumber + 'TotalActCF').value = ((ActSFCFLF) * TotalLF).round(2);
        if (document.getElementById('rowFP' + rownumber + 'TotalActCF'))
            document.getElementById('rowFP' + rownumber + 'TotalActCF').value = ((ActSFCFLF ) * TotalLF).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalActCF').value = "";
        if (document.getElementById('rowFP' + rownumber + 'TotalActCF'))
            document.getElementById('rowFP' + rownumber + 'TotalActCF').value = "";
    }
    CalculateTotalActual(rownumber);
}

function CalculateNomCFLF(rownumber) {
    var Width = document.getElementById('row' + rownumber + 'WidthHidden').value;
    var Height = document.getElementById('row' + rownumber + 'HeightHidden').value;

    if (Width !== "" && Height !== "") {
        document.getElementById('row' + rownumber + 'NomCFLF').value = ((Width * Height) / 144).round(2);
        if (document.getElementById('rowFP' + rownumber + 'NomCFLF'))
            document.getElementById('rowFP' + rownumber + 'NomCFLF').value = ((Width * Height) / 144).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'NomCFLF').value = "";
        if (document.getElementById('rowFP' + rownumber + 'NomCFLF'))
            document.getElementById('rowFP' + rownumber + 'NomCFLF').value = "";
    }
    CalculateTotalNomCF(rownumber);
}

function CalculateNomCFPcs(rownumber) {
    var Length = document.getElementById('row' + rownumber + 'LengthHidden').value;
    var NomCFLF = document.getElementById('row' + rownumber + 'NomCFLF').value;

    if (Length !== "" && NomCFLF !== "") {
        document.getElementById('row' + rownumber + 'NomCFPcs').value = (Length * NomCFLF).round(2);
        if (document.getElementById('rowFP' + rownumber + 'NomCFPcs'))
            document.getElementById('rowFP' + rownumber + 'NomCFPcs').value = (Length * NomCFLF).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'NomCFPcs').value = "";
        if (document.getElementById('rowFP' + rownumber + 'NomCFPcs'))
            document.getElementById('rowFP' + rownumber + 'NomCFPcs').value = "";
    }
     CalculateTotalNomCF(rownumber);
}

function CalculateLengthFromPCS(rowNumber) {

    let TotalLF = document.getElementById('row' + rowNumber + 'TotalLF').value;
    let TotalPcs = document.getElementById('row' + rowNumber + 'Pieces').value ;

    if (TotalLF && TotalPcs) {
        let Length = ((TotalLF / TotalPcs) * 12).round(2);
        document.getElementById('row' + rowNumber + 'Length').value = Length;
        ConvertNumberToFraction(document.getElementById('row' + rowNumber + 'Length'));
        
    }

}
function CalculatePCS(rowNumber) {
    let TotalLF = document.getElementById('row' + rowNumber + 'TotalLF').value;
    let Length = document.getElementById('row' + rowNumber + 'LengthHidden').value;

    if (TotalLF && Length) {
        let TotalPcs = (TotalLF / Length).round(2);
        document.getElementById('row' + rowNumber + 'Pieces').value = TotalPcs;
    }

}
function CalculateTotalNomCF(rownumber) {
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var NomCFLF = document.getElementById('row' + rownumber + 'NomCFLF').value;

    if (TotalLF !== "" && NomCFLF !== "") {
        document.getElementById('row' + rownumber + 'TotalNomCF').value = (TotalLF * NomCFLF).round(2);
        if (document.getElementById('rowFP' + rownumber + 'TotalNomCF'))
            document.getElementById('rowFP' + rownumber + 'TotalNomCF').value = (TotalLF * NomCFLF).round(2);
        CalculateTotalNominal(rownumber);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalNomCF').value = "";
        if (document.getElementById('rowFP' + rownumber + 'TotalNomCF'))
        document.getElementById('rowFP' + rownumber + 'TotalNomCF').value = "";
    }
}

function CalculateActSFCFLF(rownumber) {
    var Width = document.getElementById('row' + rownumber + 'WidthHidden').value;
    var TotalLF = document.getElementById('row' + rownumber + 'TotalLF').value;
    var Length = document.getElementById('row' + rownumber + 'LengthHidden').value;

    if (TotalLF !== "" && Width !== "" && Length !== "") {
        document.getElementById('row' + rownumber + 'ActSFCFLF').value = (Length * Width / 144 * TotalLF).round(2);
    }
    else
        document.getElementById('row' + rownumber + 'ActSFCFLF').value = "";
    CalculateActCFPcs(rownumber);
    CalculateTotalActCF(rownumber);
}

function CalculateActCFPcs(rownumber) {
    var length = document.getElementById('row' + rownumber + 'LengthHidden').value;
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
    var ActualCF = document.getElementById('actualCfForFinalTab').value;

    if (TotalActCF !== "" && ActualCF !== "") {
        document.getElementById('row' + rownumber + 'TotalActual').value = (TotalActCF * ActualCF).round(2);
        if (document.getElementById('rowFP' + rownumber + 'TotalActual'))
        document.getElementById('rowFP' + rownumber + 'TotalActual').value = (TotalActCF * ActualCF).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalActual').value = "";
        if (document.getElementById('rowFP' + rownumber + 'TotalActual'))
        document.getElementById('rowFP' + rownumber + 'TotalActual').value = "";
    }
}

function CalculateTotalActualOnChangeOfActualCF() {
    
    var ActualCF = document.getElementById('actualCfForFinalTab').value;
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
    var NominalCF = document.getElementById('nominalCFForFinalTab').value;

    if (TotalNomCF !== "" && NominalCF !== "") {
        document.getElementById('row' + rownumber + 'TotalNominal').value = (TotalNomCF * NominalCF).round(2);
        if (document.getElementById('rowFP' + rownumber + 'TotalNominal'))
        document.getElementById('rowFP' + rownumber + 'TotalNominal').value = (TotalNomCF * NominalCF).round(2);
    }
    else {
        document.getElementById('row' + rownumber + 'TotalNominal').value = "";
        if (document.getElementById('rowFP' + rownumber + 'TotalNominal'))
        document.getElementById('rowFP' + rownumber + 'TotalNominal').value = "";
    }
}

function CalculateTotalNominalOnChangeOfNominalCF() {

    var NominalCF = document.getElementById('nominalCFForFinalTab').value;
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


function CallSumMethods() {
    SumTotalOfActual();
    SumTotalOfNominal();
    SumLineItemCharge();
    
    SumOfPcs();
    SumOfTotalActualCF();
    SumOfTotalNomCF();
    SumOfTotalLF();
    SumOfLineItemChargeOnChange();
    SumLineItemChargePCs();
    SumLineItemChargeCF();
}
function SumTotalOfActual() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalActual = document.getElementById('rowFP' + rownumber + 'TotalActual').value;
        if (TotalActual != "")
            sum = sum + parseFloat( TotalActual);
    }
    document.getElementById('rowFPSumTotalActual').value = sum.round(2)
}

function SumTotalOfNominal() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalNominal = document.getElementById('rowFP' + rownumber + 'TotalNominal').value;
        if (TotalNominal != "")
            sum = sum + parseFloat(TotalNominal);
    }
    document.getElementById('rowFPSumTotalNominal').value = sum.round(2)
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
    var TotalLF = document.getElementById('rowFP' + rownumber + 'TotalLF').value;
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

    if (LineItemCharge != "" && TotalLF != "") {
        document.getElementById('rowFP' + rownumber + 'LineItemChargeLF').value = (LineItemCharge / TotalLF).round(2);
    }

    SumLineItemChargePCs();
    SumLineItemChargeCF();
}

function SumLineItemCharge() {
    var rowCount = $('.exceldatarow1').length;

    var LineItemTotal = document.getElementById('LineItemTotal').value;
    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var LineItemCharge = document.getElementById('rowFP' + rownumber + 'LineItemCharge').value;
        if (LineItemCharge != "")
            sum = sum + parseFloat(LineItemCharge);
    }
    if (LineItemTotal != "" && sum != "")
        document.getElementById('SumOfLineItemCharge').value = (sum - LineItemTotal).round(2);
}

function SumLineItemChargePCs() {
    var rowCount = $('.exceldatarow1').length;
    var rowFPSumOfPcs = document.getElementById('rowFPSumOfPcs').value;
    var LineItemTotal = document.getElementById('LineItemTotal').value;
    
    if (LineItemTotal != "" && rowFPSumOfPcs != "")
        document.getElementById('rowFPSumLineItemChargePCs').value = (LineItemTotal / rowFPSumOfPcs).round(2);
}

function SumLineItemChargeCF() {
    var rowCount = $('.exceldatarow1').length;

    
    var LineItemTotal = document.getElementById('LineItemTotal').value;
    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var LineItemCharge = document.getElementById('rowFP' + rownumber + 'LineItemCharge').value;
        var TotalCheckBox = $("input[name='rowFP" + rownumber + "TotalCheckBox']:checked").val();

        if (TotalCheckBox == "1") {
            sum = sum + parseFloat(document.getElementById('rowFP' + rownumber + 'TotalActCF').value);
        }
        else
            sum = sum + parseFloat(document.getElementById('rowFP' + rownumber + 'TotalNomCF').value);
    }
    if ( sum != "")
        document.getElementById('rowFPSumOfLineItemChargeCF').value = (LineItemTotal/sum).round(2);
}

function ConvertNumberToFraction(input) {
    var hiddenInput = document.getElementById(input.id + "Hidden");
    if (hiddenInput)
        hiddenInput.value = input.value;
    var numberDecimal = Math.abs(input.value);
    var decimalValue = numberDecimal - Math.floor(numberDecimal);
    var intergerValue = numberDecimal - (numberDecimal - Math.floor(numberDecimal));
    if (decimalValue > 0) { 
        var decimalNumber = GetUpperLimitOfDecimalToGetFractionValue(decimalValue);
        input.value = intergerValue + " " + GetFraction(decimalNumber);
    }
}

function SumOfPcs() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var Pieces = document.getElementById('rowFP' + rownumber + 'Pieces').value;
        if (Pieces != "")
            sum = sum + parseFloat(Pieces);
    }
    if (sum != "")
        document.getElementById('rowFPSumOfPcs').value = (sum).round(2);
}

function SumOfTotalActualCF() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalActCF = document.getElementById('rowFP' + rownumber + 'TotalActCF').value;
        if (TotalActCF != "")
            sum = sum + parseFloat(TotalActCF);
    }
    if (sum != "")
        document.getElementById('rowFPSumOfTotalActualCF').value = (sum).round(2);
}

function SumOfTotalNomCF() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalNomCF = document.getElementById('rowFP' + rownumber + 'TotalNomCF').value;
        if (TotalNomCF != "")
            sum = sum + parseFloat(TotalNomCF);
    }
    if (sum != "")
        document.getElementById('rowFPSumOfTotalNomCF').value = (sum).round(2);
}

function SumOfTotalLF() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var TotalLF = document.getElementById('rowFP' + rownumber + 'TotalLF').value;
        if (TotalLF != "")
            sum = sum + parseFloat(TotalLF);
    }
    if (sum != "")
        document.getElementById('rowFPSumOfTotalLF').value = (sum).round(2);
}

function SumOfLineItemChargeOnChange() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var LineItemCharge = document.getElementById('rowFP' + rownumber + 'LineItemCharge').value;
        if (LineItemCharge != "")
            sum = sum + parseFloat(LineItemCharge);
    }
    if (sum != "")
        document.getElementById('rowFPSumLineItemCharge').value = (sum).round(2);
}

function GetUpperLimitOfDecimalToGetFractionValue(inputNumber) {
    var selectedNumber = 0;
    var listOfDecimalValues = new Array();
    listOfDecimalValues.push(0.125);
    listOfDecimalValues.push(0.250);
    listOfDecimalValues.push(0.375);
    listOfDecimalValues.push(0.500);
    listOfDecimalValues.push(0.625);
    listOfDecimalValues.push(0.750);
    listOfDecimalValues.push(0.875);
    listOfDecimalValues.push(0.0625);
    listOfDecimalValues.push(0.1875);
    listOfDecimalValues.push(0.3125);
    listOfDecimalValues.push(0.4375);
    listOfDecimalValues.push(0.5625);
    listOfDecimalValues.push(0.6875);
    listOfDecimalValues.push(0.8125);
    listOfDecimalValues.push(0.9375);
    listOfDecimalValues.sort(function (a, b) { return a - b; });

    listOfDecimalValues = listOfDecimalValues.sort(function (a, b) { return a - b; });
    // Log to console
    console.log(listOfDecimalValues)

    for (var i = 0; i < listOfDecimalValues.length; i++) {
        if (i + 1 < listOfDecimalValues.length) {
            if (inputNumber >= listOfDecimalValues[i] || inputNumber <= listOfDecimalValues[i + 1]) {
                if (inputNumber == listOfDecimalValues[i]) {
                    selectedNumber = listOfDecimalValues[i];
                    console.log(listOfDecimalValues[i]);
                    break;
                }
                if (inputNumber <= listOfDecimalValues[i + 1]) {
                    selectedNumber = listOfDecimalValues[i + 1];
                    console.log(listOfDecimalValues[i + 1]);
                    break;
                }
            }
        }
        else {
            console.log(listOfDecimalValues[i + 1]);
            selectedNumber = listOfDecimalValues[i + 1];
        }
    }

    return selectedNumber;
}


function GetFraction(decimalNumber) {
    if (decimalNumber == 0.125)
        return "1/8";
    if (decimalNumber == 0.250)
        return "1/4";
    if (decimalNumber == 0.375)
        return "3/8";
    if (decimalNumber == 0.500)
        return "1/2";
    if (decimalNumber == 0.625)
        return "5/8";
    if (decimalNumber == 0.750)
        return "3/4";
    if (decimalNumber == 0.875)
        return "7/8";
    if (decimalNumber == 0.0625)
        return "1/16";
    if (decimalNumber == 0.1875)
        return "3/16";
    if (decimalNumber == 0.3125)
        return "5/16";
    if (decimalNumber == 0.4375)
        return "7/16";
    if (decimalNumber == 0.5625)
        return "9/16";
    if (decimalNumber == 0.6875)
        return "11/16";
    if (decimalNumber == 0.8125)
        return "13/16";
    if (decimalNumber == 0.9375)
        return "15/16";

}

function GetDecimalFromFraction(fractionNumber) {
    if (fractionNumber == "1/8")
        return 0.125;
    if (fractionNumber == "1/4")
        return 0.250;
    if (fractionNumber == "3/8")
        return 0.375;
    if (fractionNumber == 0.500)
        return "1/2";
    if (fractionNumber == 0.625)
        return "5/8";
    if (fractionNumber == 0.750)
        return "3/4";
    if (fractionNumber == 0.875)
        return "7/8";
    if (fractionNumber == 0.0625)
        return "1/16";
    if (fractionNumber == 0.1875)
        return "3/16";
    if (fractionNumber == 0.3125)
        return "5/16";
    if (fractionNumber == 0.4375)
        return "7/16";
    if (fractionNumber == 0.5625)
        return "9/16";
    if (fractionNumber == 0.6875)
        return "11/16";
    if (fractionNumber == 0.8125)
        return "13/16";
    if (fractionNumber == 0.9375)
        return "15/16";

}

function AddNewLFRow() {
    //dialog.dialog("open");
}



function DeleteProjectDetail(idValue,rowId) {
    var dataDto = { Id : idValue };

    if (confirm("Are you sure you want to delete row?")) {

        $.ajax({
            type: 'POST',
            url: "/Home/DeleteRow",
            'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: dataDto,
            success: function (resultData) {
                $('#mainrowdiv' + rowId).remove();
            }
        });
       
    }
}

function DeletePlanElevationText(id) {
    var dataDto = { Id: id };

    if (confirm("Are you sure you want to delete row?")) {

        $.ajax({
            type: 'POST',
            url: "/Home/DeletePlanElevationText",
            'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: dataDto,
            success: function (resultData) {
                window.location.reload();
            },            
        });
        
    }
}

function AddNewRowForPlanElevationDropDown() { 
    var planelevationtextrows = $('.planelevationtextrows').length;
    planelevationtextrows = planelevationtextrows + 1;
    $('.planElevationTextForEdit').append(
        '<div class="row planelevationtextrows">' +
        '<div class="col-1"></div>'+
        '<label for="" class="col-2">Plan Elevation Text:</label>' +
        '<div class="col-2 text-center" >' +
        '<input type="text" id="PlanElevationTextInput' + planelevationtextrows + '" name = "PlanElevationTextInput' + planelevationtextrows + '" db-id="0" class= "form-control" /> ' +
        '</div>' +
        '<div  class="col-2 text-center"  onpaste="paste(event)" ondrop="drop(event)" ondragover="allowDrop(event)" id="' + planelevationtextrows + '">' +        
        '<a id="ImageSnipLink' + planelevationtextrows + '" href = "" target = "_blank" > <img src="" id="ImageSnip' + planelevationtextrows + '" style="width:100%;" /> </a >' +
        '<input type="file" class="fileUploads" id="ImageSnipFile' + planelevationtextrows + '" name="ImageSnipFile' + planelevationtextrows + '" accept="image/*" style="display:none;"/> <i class="fa fa-upload fa-2x" aria-hidden="true" style="cursor:pointer;" onclick="OpenFileSelectionDialog("ImageSnipFile' + planelevationtextrows + '")"></i> <br/>Select or Drop Image' +
        '</div>' +
        '<div class="col-2 text-center" onpaste="paste1(event)" ondrop="drop1(event)" ondragover="allowDrop(event)" >' +
        '<a id="ImageSnipLink' + planelevationtextrows + '" href = "" target = "_blank" > <img src="" id="imagePageRef' + planelevationtextrows + '" style="width:100%;" /> </a >' +
        '<input type="file" class="fileUploads" id="ImagePageRefFile' + planelevationtextrows + '" name="ImagePageRefFile' + planelevationtextrows + '" accept="image/*" style="display:none;"/> <i class="fa fa-upload fa-2x" aria-hidden="true" style="cursor:pointer;" onclick="OpenFileSelectionDialog("ImagePageRefFile' + planelevationtextrows + '")"></i> <br/>Select or Drop Image' +
        '</div>'+
        '<div class="col-sm-1" style="align-self: normal">' +
        '<a href="#" onclick="DeletePlanElevationNewRow()"><i class="fa fa-trash fa-lg"></i></a>'+
        '</div>'+
        '</div>');
}

function AddPlanElevationRow() {
    var rowCount = $('.planelevation').length+1;

    $('#rows').append(
        '<div class="row" id="rowelevation' + rowCount + '">' +
        '<div class= "col-1"><a href="#" onclick="DeletePlanElevationRow(' + rowCount + ');"><i class="fa fa-trash fa-lg"></i></a> ' + rowCount + '<input type="hidden" id="PlanElevationReferanceId' + rowCount + '" name="PlanElevationReferanceId' + rowCount + '" value="-' + rowCount + '" /></div> ' +
        '<div class= "col-2"><select id="planelevation' + rowCount + '" rowid="' + rowCount + '" class="form-select planElevationDropDown" onchange="OnChangeOfPlanElevationTextDropdown(' + rowCount + ')" /></select></div> ' +
        '<div class= "col-1" > <input type="text" id="lf' + rowCount + '" name="lf' + rowCount + '" class="form-control" /></div> ' +
        '<div class= "col-1" > <input type="text" id="pcs' + rowCount + '" name="pcs' + rowCount + '" class="form-control" /></div> ' +
        '<div class= "col-1" ></div><div class= "col-5" ><div class="row"> ' +
        '<div class= "col-6"> <a id="pElevationImage' + rowCount + '" href = "" target = "_blank" > <img src="" id="imageSnip' + rowCount + '" style="width:100px;" /> </a > </div > ' +        
        '<div class= "col-6"> <a id="pElevationImagePageRef' + rowCount + '" href = "" target = "_blank" > <img src="" id="imagePageRef' + rowCount + '" style="width:100px;" /> </a > </div > ' + 
        ' </div></div> ' +
        '</div>');

    var dropDownValuesJson = document.getElementById('planElevationTextHidden').value;

    var dropDownValuesArray = JSON.parse(dropDownValuesJson);

    for (var index = 0; index < dropDownValuesArray.length; index++) {
        var key = '#planelevation' + rowCount;
        var ele = $(key);
        $(key).append(
            '"<option value = ' + dropDownValuesArray[index].Id + '> ' + dropDownValuesArray[index].Text + '</option > ');        
    }

}

function allowDrop(ev) {
    ev.preventDefault();
}

function drop(ev) {
    ev.preventDefault();
    var id = ev.currentTarget.id;
    var data = ev.dataTransfer.files;
    //ev.target.appendChild(document.getElementById(data));
    document.getElementById('ImageSnipFile' + id).files = data;
    var file = data[0];
    if (file) {
        var filereader = new FileReader();
        filereader.readAsDataURL(file);
        filereader.onload = function (evt) {
            var base64 = evt.target.result;
            $('#ImageSnip' + id).prop('src', base64);
            $('#ImageSnip' + id).show();
        }
    }
}

function drop1(ev) {
    ev.preventDefault();
    var id = ev.currentTarget.id;
    var data = ev.dataTransfer.files;
    //ev.target.appendChild(document.getElementById(data));
    document.getElementById('PageRefFile' + id).files = data;
    var file = data[0];
    if (file) {
        var filereader = new FileReader();
        filereader.readAsDataURL(file);
        filereader.onload = function (evt) {
            var base64 = evt.target.result;
            $('#PageRef' + id).prop('src', base64);
            $('#PageRef' + id).show();
        }
    }
}

function paste(ev) {
    ev.preventDefault();
    var id = ev.currentTarget.id;
    var data = ev.clipboardData.files;
    //ev.target.appendChild(document.getElementById(data));
    document.getElementById('ImageSnipFile' + id).files = data;    
    var file = data[0];
    if (file) {
        var filereader = new FileReader();
        filereader.readAsDataURL(file);
        filereader.onload = function (evt) {
            var base64 = evt.target.result;
            $('#ImageSnip' + id).prop('src', base64);
            $('#ImageSnip' + id).show();
        }
       
    }
}

function paste1(ev) {
    ev.preventDefault();
    var id = parseInt(ev.currentTarget.id);
    var data = ev.clipboardData.files;

    document.getElementById('PageRefFile' + id).files = data;
    var file = data[0];
    if (file) {
        var filereader = new FileReader();
        filereader.readAsDataURL(file);
        filereader.onload = function (evt) {
            var base64 = evt.target.result;
            $('#PageRef' + id).prop('src', base64);
            $('#PageRef' + id).show();
        }

    }
}

function GetImageByteArray(file) {
    var fileReader = new FileReader();
    var imageArray = null;
    if (fileReader) {
        fileReader.readAsArrayBuffer(file);
        fileReader.onload = function () {
            return fileReader.result;
        };        
    }
}

function fileToByteArray(file) {
    return new Promise((resolve, reject) => {
        try {
            let reader = new FileReader();
            let fileByteArray = [];
            reader.readAsArrayBuffer(file);
            reader.onloadend = (evt) => {
                if (evt.target.readyState == FileReader.DONE) {
                    fileByteArray = evt.target.result;
                    
                }
                resolve(fileByteArray);
            }
        }
        catch (e) {
            reject(e);
        }
    });
}

function GetImageBase64(file) {
    
    if (file) {
        var filereader = new FileReader();
        filereader.readAsDataURL(file);
        filereader.onload = function (evt) {
            var base64 = evt.target.result;
            return base64;
        }
    }
}

function OpenFileSelectionDialog(fileInputId) {
    document.getElementById(fileInputId).click();
}

function ShowPlanElevationFileSelection(rowNumber) {

    document.getElementById('ImageSnipFile' + rowNumber).click();
}

function ShowImagePageRefFileSelection(rowNumber) {

    document.getElementById('imagePageRef' + rowNumber).click();
}
function CloseModal() {
    $('#exampleModal').modal('hide');
}

function CalculateLF() {
    var rowCount = $('.planelevation').length;

    var planElevationJsonString = document.getElementById('row' + rowIndex + 'PlanElevationJsonHidden').value;
    var planElevationJsonArray = [];
    if (planElevationJsonString != "")
        planElevationJsonArray = JSON.parse(planElevationJsonString);

    var sum = 0;
    var pcsSum = 0;

    
    for (var i = 1; i <= rowCount; i++) {
        if ($('#lf' + i).val() != "") {
            var planElevObj = planElevationJsonArray.find(({ Id }) => Id == $('#PlanElevationReferanceId' + i).val());
            var planElevIndex = planElevationJsonArray.findIndex(x => x.Id == $('#PlanElevationReferanceId' + i).val());
            var file = $('#planElevationFile' + i).prop('files');
            var ifile = $('#imagePageRef' + i).prop('files');
            sum = sum + parseFloat($('#lf' + i).val());
            pcsSum = pcsSum + parseFloat($('#pcs' + i).val());
            
           
            if (planElevObj !== undefined) {
                planElevObj.LFValue = $('#lf' + i).val();
                planElevObj.PlanElevationValue = $('#planelevation' + i).val();
                planElevObj.PcsValue = $('#pcs' + i).val();
                
                planElevationJsonArray[planElevIndex] = planElevObj;
            }
            else {
                planElevObj = {
                    Id: parseInt($('#PlanElevationReferanceId' + i).val()),
                    LFValue: $('#lf' + i).val(),
                    PlanElevationValue: $('#planelevation' + i).val(),
                    PcsValue: $('#pcs' + i).val(),
                    
                };                
                
                planElevationJsonArray.push(planElevObj);
            }
        }
    }
    document.getElementById('row' + rowIndex + 'PlanElevationJsonHidden').value = JSON.stringify(planElevationJsonArray);
    if (sum != "") { 
        document.getElementById('row' + rowIndex + 'TotalLF').value = (sum).round(2);
    }

    document.getElementById('row' + rowIndex + 'Pieces').value = (pcsSum).round(2); 
   
   
    UploadImages(planElevationJsonArray);
    CalculateLengthFromPCS(rowIndex)
    $('#exampleModal').modal('hide');
}

function CreateGuid() {
    function _p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
function UploadImages(pElevationArray) {
    
    var id = $('#ProjectDetailIdHidden').val();
    var formData = new FormData();
    
    formData.append("pElevationJsonArray", JSON.stringify(pElevationArray));

    $.ajax({
        url: "/PlanElevation/UploadImages/?projectDetailId=" + id,
        type: "POST",       
        data: formData,
        processData: false,
        contentType: false,
        async: false,
        enctype: 'multipart/form-data',
        success: function (response) {
            var responseArray = JSON.parse(response);
            document.getElementById('row' + rowIndex + 'PlanElevationJsonHidden').value = JSON.stringify(responseArray);                                
        },
        error: function (er) {
            alert(er.responseText);
        }

    });

}

async function SaveRowForPlanElevationText(projectId) {

    var rows = $('.planelevationtextrows').length;
    if (rows == 0)
        return;
    var editPlanTextList = [];
    var formData = new FormData();

    for (var i = 1; i <= rows; i++) {

        var imageSnipFileData = $('#ImageSnipFile' + i).prop('files');
        var pageRefFileData = $('#PageRefFile' + i).prop('files');

        var guid = null; 
        var pageRefGuid = null; 

        if (imageSnipFileData && imageSnipFileData.length > 0)
            guid = CreateGuid() + '-' + imageSnipFileData[0].name;
        if (pageRefFileData && pageRefFileData.length > 0)
            pageRefGuid = CreateGuid() + '-' + pageRefFileData[0].name;

        let palnElevationText = {
            text : $('#PlanElevationTextInput' + i).val(),
            id: $('#PlanElevationTextInput' + i).attr('db-id'),
            imageSnipPath: guid,
            PageRefImagePath: pageRefGuid,
            projectId: projectId
        };

        editPlanTextList.push(palnElevationText);

        if (imageSnipFileData && imageSnipFileData.length > 0)
            formData.append("imageSnipFiles", imageSnipFileData[0], guid);
        if (pageRefFileData && pageRefFileData.length > 0)
            formData.append("pageRefFiles", pageRefFileData[0], pageRefGuid);
    }
       
    formData.append("planElevationTextRequests", JSON.stringify(editPlanTextList) );
    $.ajax({
        url: '/Home/SavePlanElevationText/',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        enctype: 'multipart/form-data',
        success: function (response) {
            
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}
function DeletePlanElevationNewRow() {
    $('.planElevationText').children().last().remove();
}
function DeletePlanElevationRow(idValue) {
    if (confirm("Are you sure to delete Plan/Elevation?")) { 
    var PlanElevationReferanceId = $('#rowelevation' + idValue).find('input[id=PlanElevationReferanceId' + idValue+']');
    var dataDto = { Id: PlanElevationReferanceId.val() };
        if (PlanElevationReferanceId.val() > 0) {
        $.ajax({
            url: "/Home/DeleteProjectPlanElevation/",
            type: "POST",
            data: dataDto,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            success: function (response) {

            },
            error: function (er) {
               
            }

        });
    }
        $('#rowelevation' + idValue).remove();
    }
}

function ArchiveProjects() {
    document.forms[0].action = "Archive";
    var $boxes = $('input[name=ArchiveActive]:checked');
    if ($boxes.length > 0)
        document.forms[0].submit();
    else
        document.forms[0].preventDefault();
}

function ActiveProjects() {
    document.forms[0].action = "Active";
    var $boxes = $('input[name=ArchiveActive]:checked');
    if ($boxes.length > 0)
        document.forms[0].submit();
    else
        document.forms[0].preventDefault();
}

