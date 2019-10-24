// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function checkCanSend() {
    var sendButton = document.querySelector('.team-estimate__send-button');

    var form = document.getElementById('team-estimate-form');
    var radioGroups = document.querySelectorAll('.team-estimate-criteria__marks');
    var checkedRadiosCount = calcRadioCheckedCount(form);
    var textAreas = document.querySelectorAll('.team-estimate-criteria__area__text');
    var filledAreasCount = calcFilledAreasCount(form);

    var isValidForm = radioGroups.length === checkedRadiosCount && textAreas.length === filledAreasCount;
    isValidForm ? sendButton.classList.remove('disabled') : sendButton.classList.add('disabled');
}

function sendData() {
    var categories = [];
    var textAreas = document.querySelectorAll('.team-estimate-criteria__area__text');
    textAreas.forEach(p => {
        console.log(p);
        categories.push({
            Id: p.getAttribute('catId'),            
            Value: p.value
        });
    });

    //var radioGroups = document.querySelectorAll('.team-estimate-criteria__marks');
    //radioGroups.forEach( p=>
    //    categories.add({
    //        Id: p.getAttribute('catId'),
    //        Name: p.getAttribute('name'),
    //        Kind: 0,
    //        Value: form.getElementsByTagName('input').first(p=>p.checked).index
    //}));
    //}

    var result = {
        TeamId: 0,
        TeamName: "",
        Categories: categories
    };

    $.ajax({
        url: '/team/score',
        dataType: 'json',
        type: 'post',
        contentType: 'application/json',
        data: JSON.stringify({
            TeamId: 1,
            TeamName: "kek",
            Categories: categories
        }),
        processData: false,
        success: function(data, textStatus, jQxhr) {
            $(window.location('/'));
        },
        error: function(jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

    //var xhr = new XMLHttpRequest();
    //xhr.open("POST", '/team/score', true);
    //xhr.setRequestHeader('Content-Type', 'application/json');
    //xhr.onload = function () {
    //    console.log('kekekke');
    //    //    window.location('/');
    //};

    //xhr.send(JSON.stringify({
    //    TeamId: 1,
    //    TeamName: "kek",
    //    Categories: categories
    //}));


function calcRadioCheckedCount(form) {
    var count = 0;
    var inputs = form.getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            count++;
        }
    }
    return count;
}

function calcFilledAreasCount(form) {
    var count = 0;
    var areas = form.getElementsByTagName('textarea');
    for (var i = 0; i < areas.length; i++) {
        if (areas[i].value) {
            count++;
        }
    }
    return count;
}