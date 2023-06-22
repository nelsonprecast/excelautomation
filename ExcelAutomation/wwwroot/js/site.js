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
    var rowCount = $('.exceldatarow').length + 1;
    $.ajax({
        url: '/Home/GetProjectDetailView/?id=' + rowCount,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html',
        success: function (response) {
            $('#RowsTable').append(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
   
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
    var ActualCF = document.getElementById('ActualCF').value;

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

    var LineItemTotal = document.getElementById('LineItemTotal').value;
    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var LineItemChargePCs = document.getElementById('rowFP' + rownumber + 'LineItemChargePCs').value;
        if (LineItemChargePCs != "")
            sum = sum + parseFloat(LineItemChargePCs);
    }
    if ( sum != "")
        document.getElementById('rowFPSumLineItemChargePCs').value = (sum - LineItemTotal).round(2);
}

function SumLineItemChargeCF() {
    var rowCount = $('.exceldatarow1').length;

    var sum = 0;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        var LineItemChargeCF = document.getElementById('rowFP' + rownumber + 'LineItemChargeCF').value;
        if (LineItemChargeCF != "")
            sum = sum + parseFloat(LineItemChargeCF);
    }
    if ( sum != "")
        document.getElementById('rowFPSumOfLineItemChargeCF').value = (sum).round(2);
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