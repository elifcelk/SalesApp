$(document).ready(function () {
    var dropdown = document.getElementById("storeId");
    var selectedValue = dropdown.value;

    $("#viewButton").click(function () {
        console.log(selectedValue);
    });
});