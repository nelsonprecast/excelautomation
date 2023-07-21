var rowIndex = 2;
$(document).ready(function () {
    $('#datepicker').datepicker({
        autoclose: true
    });

    BindEvent();

    
});

$(function () {
    function log(message) {
        //$("<div>").text(message).prependTo("#log");
        //$("#log").scrollTop(0);
    }

    $("#projectname").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/GetJobNameList/",
                dataType: "json",
                data: {
                    searchString: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.displayName,
                            value: item.displayName,
                            id: item.id,
                            accountName: item.account_name
                        };
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {            
            $('#opportunityId').val(ui.item.id);
            $('#accountName').val(ui.item.accountName);

        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
});

function AddNewRowForPlanElevationDropDown() {    
    $('.planElevationText').append(
        '<div class="input-group">'+
        '<label for="" class="col-sm-3">Plan Elevation Text:</label>' +
        '<div class="col-sm-2" >' +
        '<input type="text" id="planElevationTextRow' + rowIndex + '" name = "planElevationTextRow' + rowIndex + '" class= "form-control" /> ' +
        '</div>' +
        '</div>');
    rowIndex = rowIndex+1;
}

function BindEvent() {
    $('.exceldatarow11').click(function () {

        console.log($(this));
        $(this).find('.totallfdata').each(function (index, data) {
            rowIndex = $(data).find('#rowIndex').first().val();
            var planElevationString = document.getElementById('row' + rowIndex + 'PlanElevationHidden').value;
            var totalLFString = document.getElementById('row' + rowIndex + 'TotalLFHidden').value;
            $('#rows').html('');

            console.log(planElevationString);
            console.log(totalLFString);

            if (planElevationString != "") {
                var planElevationArray = planElevationString.split("@_@");
                var totalLFArray = totalLFString.split("@_@");
                if (planElevationArray.length > 0) {

                    for (var i = 0; i < planElevationArray.length; i++) {
                        AddPlanElevationRow();
                        $('#planelevation' + (i + 1)).val(planElevationArray[i]);
                        $('#lf' + (i + 1)).val(totalLFArray[i]);
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