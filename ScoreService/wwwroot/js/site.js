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
    for (var i = 0; i < textAreas.length; i++) {
        categories.add({
            Id: textAreas[i].getAttribute('catId'),
            Name: textAreas[i].getAttribute('name'),
            Kind: 1,
            Value: textAreas[i].value
        });
    }
    var radioGroups = document.querySelectorAll('.team-estimate-criteria__marks');
    for (var i = 0; i < radioGroups.length; i++) {
        categories.add({
            Id: radioGroups[i].getAttribute('catId'),
            Name: radioGroups[i].getAttribute('name'),
            Kind: 0,
            Value: form.getElementsByTagName('input').first(p=>p.checked).index
    });
    }

    var result = {
        TeamId: 0,
        TeamName: "",
        Categories: categories
    };

    $post('team/score', result);
}


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