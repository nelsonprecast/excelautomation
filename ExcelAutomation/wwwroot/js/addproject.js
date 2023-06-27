var rowIndex = 0;
$(document).ready(function () {
    $('#datepicker').datepicker({
        autoclose: true
    });

    BindEvent();

    
});

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
                var planElevationArray = planElevationString.split("@@_@@");
                var totalLFArray = totalLFString.split("@@_@@");
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