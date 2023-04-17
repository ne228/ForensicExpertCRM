// LoadExperts


//variables

var selectTypeExpertise = document.getElementById("typeExpertiseSelect");


//functions


function setExpertsSelect(json) {
    var arr_from_json = JSON.parse(json);

    var select = document.getElementById("expertsSelect");
    select.innerHTML = '';

    for (var i = 0; i < arr_from_json.length; i++) {

        var name = arr_from_json[i]["Name"];
        var rating = arr_from_json[i]["Rating"];
        var expertId = arr_from_json[i]["Id"];

        let newOption = new Option(`${name} ${rating}`, expertId);
        select.append(newOption);
    }
    select.selectedIndex = 0;
}

function loadExperts(typeExpertiseId) {

    $.ajax({
        type: 'GET',
        url: `/Expertises/GetExperts?typeExpertiseId=${typeExpertiseId}`,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        /*data: data,*/
        success: function (result) {

            setExpertsSelect(result);
            console.log('Saved ');
            console.log(result);
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}


//main
function main() {
    var typeExpertiseId = selectTypeExpertise.options[selectTypeExpertise.selectedIndex].value;
    loadExperts(typeExpertiseId);

    if (selectTypeExpertise != null) {

        selectTypeExpertise.onchange = function () {

            var typeExpertiseId = selectTypeExpertise.options[selectTypeExpertise.selectedIndex].value;

            loadExperts(typeExpertiseId);
        }
    }
}



main();


