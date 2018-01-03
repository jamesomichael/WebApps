$(document).ready(function () {

    $.ajax({
        url: '/Announcements/BuildAnnouncementTable',
        success: function (result) {
            $('#announcementDiv').html(result);
        }

    });

});