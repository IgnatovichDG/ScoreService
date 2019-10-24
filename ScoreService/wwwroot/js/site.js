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

    var isValidForm = radioGroups.length === checkedRadiosCount;
    isValidForm ? sendButton.classList.remove('disabled') : sendButton.classList.add('disabled');
}

function sendData() {
    var teamName = $("#teamName").attr("tName");
    var teamId = $("#teamId").attr("tId");
    var categories = [];
    var textAreas = document.querySelectorAll('.team-estimate-criteria__area__text');
    textAreas.forEach(p => {
        categories.push({
            Id: p.getAttribute('catId'),
            Value: p.value
        });
    });

    var radioGroups = $(".team-estimate-criteria__marks").get();
    radioGroups.forEach(p => {
        var inputs = p.getElementsByTagName('input');
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                categories.push({
                    Id: inputs[i].getAttribute("cid"),
                    Value: inputs[i].value
                });
            }
        }
    });

    // radioGroups.forEach(p => );

    //var radioGroups = document.querySelectorAll('.team-estimate-criteria__marks');
    //radioGroups.forEach( p=>
    //    categories.add({
    //        Id: p.getAttribute('catId'),
    //        Name: p.getAttribute('name'),
    //        Kind: 0,
    //        Value: form.getElementsByTagName('input').first(p=>p.checked).index
    //}));
    //}


    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/team/score", true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                window.location.href = '/';
            } else {
                console.error(xhr.statusText);
            }
        }
    };
    xhr.onerror = function (e) {
        console.error(xhr.statusText);
    };
    xhr.send(JSON.stringify({
        TeamId: parseInt(teamId),
        TeamName: teamName,
        Categories: categories
    }));
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