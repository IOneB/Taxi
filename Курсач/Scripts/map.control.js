
function initMap() {
    //Опции карты
    var mapOptions = {
        zoom: 13,
        mapTypeId: 'roadmap',
        center: new google.maps.LatLng(48.5, 135.1251)
    };
    //Поля автозаполнения
    var autocompletes = [];
    for (var i = 1; i < 6; i++) {
        autocompletes[i - 1] = new google.maps.places.Autocomplete(
            (document.getElementById('autocomplete' + i)),
            { types: ['geocode'] });
        autocompletes[i - 1].setComponentRestrictions({ 'country': 'ru' });
        autocompletes[i - 1].setBounds(new google.maps.LatLngBounds(new google.maps.LatLng(48.329, 134.9283), new google.maps.LatLng(48.6047, 135.1436)));

    }
    //Карта
    var map = new google.maps.Map(document.getElementById("map"), mapOptions);
    //Пробки
    var trafficLayer = new google.maps.TrafficLayer();
    //Количество точек маршрута
    var pointCount = 2;
    var trafficIsShown = false;
    var isChild = false;
    var isSoon = true;
    var isShipment = false;
    var isOptimise = true;
    //Объект для отображения DirectionResult
    var directionsDisplay = new google.maps.DirectionsRenderer();
    var directionsService = new google.maps.DirectionsService();
    directionsDisplay.setMap(map);
    //Обработчик нажатия кнопки с ID traffic - отобразить/скрыть пробки
    document.getElementById('traffic').addEventListener('click', function () {
        if (trafficIsShown === false) {
            trafficLayer.setMap(map);
            trafficIsShown = true;
        }
        else {
            trafficLayer.setMap(null);
            trafficIsShown = false;
        }
    });
    //Регулирование количества точек
    document.getElementById('plus').addEventListener('click', function () {
        if (pointCount < 5) {
            document.getElementById('autocomplete' + pointCount).style.display = 'inline-block';
            document.getElementById('house' + pointCount).style.display = 'inline-block';
            pointCount++;
        }
    });
    document.getElementById('minus').addEventListener('click', function () {
        if (pointCount > 2) {
            pointCount--;
            document.getElementById('autocomplete' + pointCount).style.display = 'none';
            document.getElementById('house' + pointCount).style.display = 'none';
        }
    });
    //ЧекБокс Детское кресло
    document.getElementById('IsChild').addEventListener('change', function () {
        isChild = document.getElementById('IsChild').checked;
    });
    //ЧекБокс срочный заказ
    document.getElementById('IsSoon').addEventListener('change', function () {
        isSoon = document.getElementById('IsSoon').checked;
    });
    //ЧекБокс Грузоверевозки
    document.getElementById('IsShipment').addEventListener('change', function () {
        isShipment = document.getElementById('IsShipment').checked;
    });
    //NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
    document.getElementById('resultats').addEventListener('click', NONONO);
    document.getElementById('resultats').addEventListener('focus', NONONO);
    document.getElementById('IsOptimise').addEventListener('change', function () {
        isOptimise = document.getElementById('IsOptimise').checked;
    });
    //Построение маршрута
    document.getElementById('result').addEventListener('click', function () {
        document.getElementById('distance').innerHTML = "";
        document.getElementById('cost').innerHTML = "";
        var cost = 100.00;
        if (!isSoon) {
            if (document.getElementById('date').value === "") {
                alert("Заполните дату");
                document.getElementById('component').style.backgroundColor = "~/Content/Cheat-cot-failure.png";
                return;
            }
            if (EarlyDateCheck()) {
                Failure("Момент времени не должен быть установлен раньше текущего");
                return;
            }
            if (AfterDateCheck()) {
                Failure("Планирование возможно не больше, чем на 3 дня");
                return;
            }
            cost += 50;
        }
        if (isOptimise)
            if (pointCount > 3)
                cost += 50;
        if (isChild)
            cost += 50;
        if (isShipment)
            cost += 50;
        var start = document.getElementById('autocomplete' + 1).value + ", " + document.getElementById('house' + 1).value;
        var end = document.getElementById('autocomplete' + 5).value + ", " + document.getElementById('house' + 5).value;
        var way = [];
        for (var index = 2; index < pointCount; index++) {
            way.push({ location: document.getElementById('autocomplete' + index).value + ", " + document.getElementById('house' + index).value });
        }
        var request = {
            origin: start,
            destination: end,
            waypoints: way,
            provideRouteAlternatives: true,
            optimizeWaypoints: isOptimise,
            travelMode: 'DRIVING'
        };
        directionsService.route(request, function (result, status) {
            if (distance === 0) {
                Failure("И смысл заказывать такси?");
            }
            else if (status === 'OK') {
                directionsDisplay.setDirections(result);
                var totaldistance = 0;
                for (var j = 0; j < result.routes[0].legs.length; j++) {
                    totaldistance += result.routes[0].legs[j].distance.value;
                }
                cost += totaldistance / 100.0;
                var str = (totaldistance / 1000.0);
                document.getElementById('distance').innerHTML = str + " км.";
                document.getElementById('cost').innerHTML = cost.toFixed(2) + " руб.";
                document.getElementById('component').style.backgroundImage = "url('../../Content/Cheat-cot.png')";
                document.getElementById('resultats').value = start + "|" + end + "|";
                for (var index = 0; index < way.length; index++) {
                    document.getElementById('resultats').value += way[index].location + "|";
                }
                for (var i = pointCount; i < 5; i++) {
                    document.getElementById('resultats').value += "null|";
                }
                document.getElementById('resultats').value += cost + "|" + str + "|" + isOptimise + "|" + isChild + "|" + isSoon + "|" + isShipment + "|" + document.getElementById('date').value;
            }
            else {
                Failure("Не удалось построить маршрут, попробуйте изменить входные данные");
            }
        });
    });
}
function NONONO() {
    document.getElementById('resultats').setAttribute('type', 'hidden');
}
function Failure(alyarma) {
    alert(alyarma);
    document.getElementById('component').style.backgroundImage = "url('../../Content/Cheat-cot-failure.png')";
}
function EarlyDateCheck() {
    var inputDate = new Date(document.getElementById('date').value);
    var now = new Date();
    now.getDate();
    if (inputDate < now)
        return true;
    return false;
}
function AfterDateCheck() {
    var inputDate = new Date(document.getElementById('date').value);
    var now = new Date();
    now.setDate(now.getDate() + 3);
    now.getDate();
    if (inputDate > now)
        return true;
    return false;
}