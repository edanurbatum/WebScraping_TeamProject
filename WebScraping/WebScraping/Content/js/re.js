var hsaModel = { t: null, c: null, v: null };
var say = 0;
$(document).ready(function () {

    $('#ilsecim').on('change', function () {
        if (!$(this).val() || parseInt($(this).val()) == '0') {
            return;
        }
        GetHHatHsData('Il?ilId=' + parseInt($(this).val()), '#ilcesecim', 'ilce', 'ilceId', parseInt($(this).val()));
    });

    $('#ilcesecim').on('change', function () {
        if (!$(this).val() || parseInt($(this).val()) == '0') {
            return;
        }
        GetHHatHsData('Ilce?ilceId=' + parseInt($(this).val()), '#mahallesecim', 'mahalle', 'mahalleId', parseInt($(this).val()));
    });



})

function GetHHatHsData(urlParam, div, t, c, v) {
    if (t) {
        hsaModel.t = t;
        hsaModel.c = c;
        hsaModel.v = v;
    }
    var selectText = 'Tümü';

    var settingshierarchical = {
        "async": true,
        "crossDomain": true,
        "url": '/Employment/' + urlParam,
        "method": "GET",
        "contentType": 'application/json',
        "dataType": 'json',
    }
    $.ajax(settingshierarchical).done(function (data) {

        var arrRes = [];

        if (t == "ilce") {
            $.each(data, function (a, b) {
                var item = { id: b.ilceId, text: b.ilce1 };
                arrRes.push(item);

            });
        }
        if (t == "mahalle") {
            $.each(data, function (a, b) {
                var item = { id: b.mahalleId, text: b.mahalle1 };
                arrRes.push(item);

            });
        }


        arrRes.unshift({ id: '-1', text: selectText });

        for (var i = 0; i < arrRes.length; i++) {
            if (t == "ilce") {
                $("#ilcesecim").append('<option value="' + arrRes[i].id + '">' + arrRes[i].text + '</option>');
            }
            if (t == "mahalle") {
                $("#mahallesecim").append('<option value="' + arrRes[i].id + '">' + arrRes[i].text + '</option>');
            }
        }


    }).fail(function (errMsg) {

    });
}


function sepeteekle(id, detay1, detay2, detay3, detay4) {
    $("#karsilastirmaList").append('<label id="' + id + '-' + detay1 + '-' + detay2 + '-' + detay3 + '-' + detay4 + '">' + detay1 + ' ili ' + detay2 + ' ilçesinde  oda sayısı : ' + detay3 + ' ve  ' + detay4 + 'm2 </label>');
}

function yenile() {
    $("#karsilastirmaList").empty();
}

function evlerikarsilastir() {
    var evlistesi = $("#karsilastirmaList label");
    if (evlistesi.length == 2) {

        var data = {
            ev1: $("#karsilastirmaList label")[0].id,
            ev2: $("#karsilastirmaList label")[1].id,
        }
        var settingshierarchical = {
            "async": false,
            "url": '/Home/Karsilastirma',
            "method": "POST",
            "contentType": 'application/json',
            "dataType": 'json',
            "data": JSON.stringify(data),
        }
        $.ajax(settingshierarchical).done(function (data) {
            window.location = "/Employment/Karsilastir";

        }).fail(function (errMsg) {

        });
    }
    else if (evlistesi < 2) {
        alert("Lütfen en az 2 ev seçerek işlem yapın");
    }
    else {
        alert("Lütfen en fazla 2 ev seçerek işlem yapın");
    }

}

function evarama() {

    var data = {
        il: $("#ilsecim option:selected").text() == "İl Seçin" ? "" : $("#ilsecim option:selected").text(),
        ilce: $("#ilcesecim option:selected").text() == "Tümü" ? "" : $("#ilcesecim option:selected").text(),
        mahalle: $("#mahallesecim option:selected").text() == "Tümü" ? "" : $("#mahallesecim option:selected").text(),
        minfiyat: $("#minfiyat").val(),
        maxfiyat: $("#maxfiyat").val(),
        minmk: $("#minmk").val(),
        maxmk: $("#maxmk").val(),
        oda: $("#odasecim option:selected").text() == "Tümü" ? "" : $("#odasecim option:selected").text(),
        yas: $("#yassecim option:selected").text() == "Tümü" ? "" : $("#yassecim option:selected").text(),
        kat: $("#katsecim option:selected").text() == "Tümü" ? "" : $("#katsecim option:selected").text(),
        totalkat: $("#totalkatsecim option:selected").text() == "Tümü" ? "" : $("#totalkatsecim option:selected").text(),
        isinma: $("#isinmasecim option:selected").text() == "Tümü" ? "" : $("#isinmasecim option:selected").text(),
        banyo: $("#banyosecim option:selected").text() == "Tümü" ? "" : $("#banyosecim option:selected").text(),
        esya: $("#esyasecim option:selected").text() == "Tümü" ? "" : $("#esyasecim option:selected").text(),

    }

    var settingshierarchical = {
        "async": true,
        "crossDomain": true,
        "url": '/Employment/EvAra',
        "method": "POST",
        "contentType": 'application/json',
        "dataType": 'json',
        "data": JSON.stringify(data),
    }
    $.ajax(settingshierarchical).done(function (data) {
        $("#evListe").empty();

        for (var i = 0; i < data.length; i++) {
            $("#evListe").append('<div class="job-cards">' +
                '<div class="media" style="border-radius: 10px;background-color: white;">' +
                '<a class="media-left media-middle" href="#">' +
                '<img class="media-object m-r-10 m-l-10" src="' + data[i].image + '" alt="' + data[i].city + '" style="width: 180px;height: 150px;margin-left: 25px;"></a>' +
                '<div class="media-body">' +
                '<div class="company-name m-b-10">' +
                '<p style="margin-top: 15px;">' + data[i].city + ' - ' + data[i].county + ' - ' + data[i].room + ' - ' + data[i].metre + '</p>' +
                '<i class="text-muted f-14"><span style="font-weight:600;">Fiyat: </span> ' + data[i].price + '</i>' +
                '</div>' +
                '<p class="text-muted">' +
                '<span style="font-weight:600;">Oda Sayısı : <span style="font-weight:100;"> ' + data[i].room + '</span></span>' +
                '<br />' +
                '<span style="font-weight:600;">Bina Yaşı :  <span style="font-weight:100;">' + data[i].age + '</span></span>' +
                '<br />' +
                '<span style="font-weight:600;">Isıtma Tipi : <span style="font-weight:100;"> ' + data[i].heating + '</span></span>' +
                '</p>' +
                '</div>' +
                '<div class="media-right" style=" vertical-align: middle;">' +
                '<div class="label-main">' +
                '<button type="submit" onclick="sepeteekle("' + data[i].ID + '","' + data[i].city + '","' + data[i].county + '","' + data[i].room + '","' + data[i].metre + '");" class="btn btn-primary" ' +
                'style="background-color: #e70000;border-radius: 10px;padding: 7px;font-size: 12px;text-transform: none;margin-right: 15px;">Karşılaştır</button>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>' +
                ' <br />');

        }

    }).fail(function (errMsg) {

    });
}