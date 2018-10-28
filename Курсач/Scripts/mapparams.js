ymaps.ready(init);

var myMap;

function init () {
    myMap = new ymaps.Map(
        'map',
        // Параметры карты.
        {
            //координаты по умолчанию (Хабаровск)
            center: [48.48, 135.064],
            //зум скроллом
            behaviors: ['default', 'scrollZoom'],
            zoom: 15,
            // Тип покрытия карты: "Схема".
            type: 'yandex#map'
        }
    );
    var suggestView1 = new ymaps.SuggestView('suggest1');
}

function setCenter () {
    myMap.setCenter([57.767265, 40.925358]);
}

function setBounds () {
    // Bounds - границы видимой области карты.
    // Задаются в географических координатах самой юго-восточной и самой северо-западной точек видимой области.
    myMap.setBounds([[37, 38], [39, 40]]);
}
