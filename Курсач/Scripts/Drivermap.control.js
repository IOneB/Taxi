function initMap() {
    //Опции карты
    var mapOptions = {
        zoom: 13,
        mapTypeId: 'roadmap',
        center: new google.maps.LatLng(48.5, 135.1251)
    };
    var map = new google.maps.Map(document.getElementById("map"), mapOptions);
}