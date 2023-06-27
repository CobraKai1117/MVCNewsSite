/*$(document).ready(function () {
    getLocation()
        .then(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            // Make an AJAX request to the controller action method
            var xhr = new XMLHttpRequest();
            xhr.open('GET', '/News/Index?latitude=' + latitude + '&longitude=' + longitude, true);
            xhr.setRequestHeader('Content-Type', 'application/json');
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
                    // Handle the response from the controller if needed
                    var response = JSON.parse(xhr.responseText);
                    // ...
                }
            };

            // Send the AJAX request
            xhr.send();
        })
        .catch(function (error) {
            // Handle the error if geolocation retrieval fails
        });
});

function getLocation() {
    return new Promise(function (resolve, reject) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(resolve, reject);
        } else {
            // Geolocation is not supported by the browser
            reject(new Error('Geolocation is not supported'));
        }
    });
} */   /// I will probably not need this if GeoIP2 will work but I will comment and leave it here just in case
// I need to circle back to it.