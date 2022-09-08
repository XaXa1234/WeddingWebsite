// Write your Javascript code.
$(document).ready(function () {
    if ($('.form-check-input').is(':checked')) {
        $(".guestinfo").show(250);
    } else {
        $(".guestinfo").hide(250);
    }
});

$('.form-check-input').click(function () {
    if ($(this).is(':checked')) {
        $(".guestinfo").show(250);
    } else {
        $(".guestinfo").hide(250);
    }
});



