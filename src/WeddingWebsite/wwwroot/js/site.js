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


function initMap() {
    const gaynes = { lat: 51.69589748304985, lng: 0.14796475731150335 };
    // The map, centered at Uluru
    const map = new google.maps.Map(document.getElementById("mapgaynespark"), {
        zoom: 14,
        center: gaynes,
    });
    // The marker, positioned at Uluru
    const marker = new google.maps.Marker({
        position: gaynes,
        map: map,
    });
}

window.initMap = initMap;
