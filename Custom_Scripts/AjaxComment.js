$(document).ready(function () {
    //$('.Comment').show(function () {
    //I used the following to do this
    //http://stackoverflow.com/questions/3730359/get-id-from-url-with-jquery
    var url = document.URL;
    var id = url.substring(url.lastIndexOf('/') + 1);
    //var id = 5;
    //var ii = 24;

    $.ajax({
        url: '/Comments/AJAXCreate/',
        data: {
            id: id
        },
        type: 'GET',
        success: function (result) {
            $('#commentsDiv').html(result);
        }
    });
    //});
});