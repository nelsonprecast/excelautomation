var rowIndex = 0;
$(document).ready(function () {
    var TabNumber = $('#TabNumber').val();
    switch (TabNumber) {
        case "1":
            activaTab('nav-project-tab');
            break;
        case "2":
            activaTab('nav-plan-elevation-tab');
            break;
        case "3":
            activaTab('nav-home-tab');
            break;
        case "4":
            activaTab('nav-profile-tab');
            break;
        case "5":
            activaTab('nav-contact-tab');
            break;
        case "6":
            activaTab('nav-pdf-proposal-tab');
            break;
    }

    $('#datepicker').datepicker({
        autoclose: true
    });
   // $('#draggableDiv').draggable();
    BindEvent();

    $(".img-container").popupLightbox();
    /* This is basic - uses default settings */
    window.console.log(1);
    CalculateTotalActualOnChangeOfActualCF();
    CalculateTotalNominalOnChangeOfNominalCF();
    var rowCount = $('.exceldatarow').length;
    for (var rownumber = 1; rownumber <= rowCount; rownumber++) {
        ConvertNumberToFraction(document.getElementById('row' + rownumber + 'Length'));
        ConvertNumberToFraction(document.getElementById('row' + rownumber + 'Width'));
        ConvertNumberToFraction(document.getElementById('row' + rownumber + 'Height'));
        CalculateActCFPcs(rownumber);
        CalculateTotalActCF(rownumber);
        CalculateNomCFLF(rownumber);
        CalculateNomCFPcs(rownumber);
        CalculateTotalNomCF(rownumber);
        OnLineItemChange(rownumber);
        //CalculatePCS(rownumber);
        //CalculateLengthFromPCS(rownumber);
    }
    CallSumMethods();
});

function BindEvent() {
    $('.exceldatarow11').click(function () {
        if ($(this).attr('readonly')) {
            $('.modal-footer').hide();
        }
        else
            $('.modal-footer').show();
        var projectDetailId = $(this).attr('rel');
        
        $(this).find('.totallfdata').each(function (index, data) {
            rowIndex = $(data).find('#rowIndex').first().val();
            var planElevationJsonString = document.getElementById('row' + rowIndex + 'PlanElevationJsonHidden').value;
            var planElevationJsonArray = [];
            if (planElevationJsonString != "")
                planElevationJsonArray= JSON.parse(planElevationJsonString);
                       
            $('#ProjectDetailIdHidden').val(projectDetailId);
            $('#rows').html('');
            
            if (planElevationJsonArray.length > 0) {               

                for (var i = 0; i < planElevationJsonArray.length; i++) {
                    AddPlanElevationRow();
                    $('#PlanElevationReferanceId' + (i + 1)).val(planElevationJsonArray[i].Id);
                    $('#planelevation' + (i + 1)).val(planElevationJsonArray[i].PlanElevationValue);
                    $('#lf' + (i + 1)).val(planElevationJsonArray[i].LFValue);
                    $('#pcs' + (i + 1)).val(planElevationJsonArray[i].PcsValue);

                    var dropDownValuesJson = document.getElementById('planElevationTextHidden').value;

                    var dropDownValuesArray = JSON.parse(dropDownValuesJson);
                    for (var index = 0; index < dropDownValuesArray.length; index++) {

                        if (dropDownValuesArray[index].Id == planElevationJsonArray[i].PlanElevationValue) {
                            if (dropDownValuesArray[index].ImageSnipPath != null) {
                                $('#imageSnip' + (i + 1)).attr('src', dropDownValuesArray[index].ImageSnipPath);
                                $('#pElevationImage' + (i + 1)).attr('href', dropDownValuesArray[index].ImageSnipPath);
                            }
                            else {
                                $('#imageSnip' + (i + 1)).attr('src', '');
                                $('#pElevationImage' + (i + 1)).attr('href', '');
                            }
                            if (dropDownValuesArray[index].PageRefImagePath != null) {
                                $('#imagePageRef' + (i + 1)).attr('src', dropDownValuesArray[index].PageRefImagePath);
                                $('#pElevationImagePageRef' + (i + 1)).attr('href', dropDownValuesArray[index].PageRefImagePath);
                            }
                            else {
                                $('#imagePageRef' + (i + 1)).attr('src', '');
                                $('#pElevationImagePageRef' + (i + 1)).attr('href', '');
                            }
                        }
                    }                        
                }               
            }
            else {
                AddPlanElevationRow();
            }
            $('#exampleModal').modal('show');
        });

    });
}

function ExportToExcelTable() {
    var rowcount = $('.exceldatarow1').length;
    var table = "<table id='testTable'> <tr> "+
        "<td>No</td>" +
        " <td>WD</td>" +
        " <td>Category</td>" +
        "<td>Item Name</td>" +
        " <td>Detail#/Page</td>" +
        " <td>Length</td>" +
        " <td>Width</td>" +
        " <td>Height</td>" +
        " <td>Pcs</td>  " + 
        " <td>Disposition (Special Notes)</td>" +
        " <td>Total LF</td>" +
        " <td>Total Nom CF</td>" +
        " <td>Nom. CF/LF</td>" +
        "<td>Act SF CF/LF</td>" +
        " <td>Act CF/Pcs</td>" +
        " <td>Total Act CF</td>" +
        " <td>Nom. CF/Pcs</td>" +
        " <td>Line Item Charge</td>" +
        "<td>Mold Worker Count</td>" +
        " <td> Drafting Hours </td>" +
        " <td> Foam </td>" +
        " <td> Production Worker Count </td>" +
        " <td> Metal Fab </td>" +
        " <td> Complexity </td>" +
        " </tr> ";
    for (var rownumber = 1; rownumber <= rowcount; rownumber++) {
        table += "<tr><td>" + rownumber + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'WD').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'Category').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'ItemName').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'DetailPage').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'Length').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'Width').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'Height').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'Pieces').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'DispositionSpecialNote').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'TotalLF').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'NomCFLF').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'TotalNomCF').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'ActSFCFLF').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'ActCFPcs').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'TotalActCF').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'NomCFPcs').value + "</td>" +
            "<td>" + document.getElementById('rowFP' + rownumber + 'LineItemCharge').value + "</td>" +
            "<td>" + document.getElementById('row' + rownumber + 'MoldWorkerCount').value + "</td>" +
            "<td>" + document.getElementById('row' + rownumber + 'DraftingHours').value + "</td>" +           
            "<td>" + document.getElementById('row' + rownumber + 'Foam').value + "</td>" +           
            "<td>" + document.getElementById('row' + rownumber + 'ProductionWorker').value + "</td>" +
            "<td>" + document.getElementById('row' + rownumber + 'MetalFab').value + "</td>" +
            "<td>" + document.getElementById('row' + rownumber + 'Complexity').value + "</td>" +


            "</tr>"
    }
    table += "</table>";
    $('#hiddenTable').html(table);
}


function exportReportToExcel() {
    let table = document.getElementById("testTable"); // you can use document.getElementById('tableId') as well by providing id to the table tag
    TableToExcel.convert(table, { // html code may contain multiple tables so here we are refering to 1st table tag
        name: `export.xlsx`, // fileName you could use any name
        sheet: {
            name: 'Sheet 1' // sheetName
        }
    });
}

function SetTab(no) {
    document.getElementById('TabNumber').value = no;
}

function activaTab(tab) {
    $('.nav-tabs button[id="' + tab + '"]').tab('show');
};

function EditGroup(groupId) {
    $('#Group' + groupId).hide();
    $('#GroupEditPencil' + groupId).hide();
    $('#GroupEdit' + groupId).show();
    $('#GroupUpdatePencil' + groupId).show();
}

function UpdateGroup(groupId) {
    var postData = {
        GroupId: groupId,
        GroupName: $('#GroupId' + groupId).val()
    }
    $.ajax({
        type: 'POST',
        url: "/Home/UpdateGroup",
        'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: postData,
        success: function (resultData) {
          
            $('#Group' + groupId).html($('#GroupId' + groupId).val());
            $('#Group' + groupId).show();
            $('#GroupEditPencil' + groupId).show();
            $('#GroupEdit' + groupId).hide();
            $('#GroupUpdatePencil' + groupId).hide();
        },
        error: function (response) {
            $('#Group' + groupId).html($('#GroupId' + groupId).val());
            $('#Group' + groupId).show();
            $('#GroupEditPencil' + groupId).show();
            $('#GroupEdit' + groupId).hide();
            $('#GroupUpdatePencil' + groupId).hide();
            alert(response.responseText);
        }
    });

}

function DeleteGroup(groupId) {
    if (confirm("Are you sure to delete group?")) {
        var postData = {
            GroupId: groupId
        }
        $.ajax({
            type: 'POST',
            url: "/Home/DeleteGroup",
            'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: postData,
            success: function (resultData) {
                window.location.reload();
            },
            error: function (response) {
                window.location.reload();
            }
        });
    }
}

function CancelEditGroup(groupId) {
    $('#Group' + groupId).show();
    $('#GroupEditPencil' + groupId).show();
    $('#GroupEdit' + groupId).hide();
    $('#GroupUpdatePencil' + groupId).hide();
}

function RemoveFromGroup() {
    var $checkBoxes = $('input:checkbox[class="GroupCheckBoxId"]:checked');
    var unGroupValues = "";
    if ($checkBoxes.length > 0) {
        for (var i = 0; i < $checkBoxes.length; i++) {
            if (unGroupValues == "")
                unGroupValues = $checkBoxes[i].value;
            else
                unGroupValues += "," + $checkBoxes[i].value;
        }

        var postData = {
            projectDetailIds: unGroupValues
        }
        $.ajax({
            type: 'POST',
            url: "/Home/RemoveFromGroup",
            'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: postData,
            success: function (resultData) {
                window.location.reload();
            },
            error: function (response) {
                window.location.reload();
            }
        });
    }
}

function SetCrmSaveBit() {
    $('#SendToCrm').val("1");
}

function OnChangeOfPlanElevationTextDropdown(rowNumber) {

    var id = $('#planelevation' + rowNumber).val();
    var dataDto = { Id: id };
    $.ajax({
        type: 'POST',
        url: "/PlanElevation/GetPlanElevationTextById",
        'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: dataDto,
        success: function (resultData) {
            if (resultData) {
                if (resultData.imageSnipPath && resultData.imageSnipPath != null) {
                    $('#imageSnip' + rowNumber).attr('src', resultData.imageSnipPath);
                }
                else {
                    $('#imageSnip' + rowNumber).attr('src', '');
                }
                if (resultData.pageRefImagePath && resultData.pageRefImagePath != null) {
                    $('#imagePageRef' + rowNumber).attr('src', resultData.pageRefImagePath);
                }
                else {
                    $('#imagePageRef' + rowNumber).attr('src', '');
                }
            }
        },
    });
}