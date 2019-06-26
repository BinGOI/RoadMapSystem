// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
        function ClickExistingUser(mentorLogin) {
            jQuery.ajax({
                url: "AddExistingIntern",
                type: "GET",
                data: "MentorLogin=" + mentorLogin + "&InternId=" + $("#EmployeeExistingSelect").val(),
                success: function () {
                    window.location.href = "Info/?login=" + mentorLogin +""
                }
            });
}



$(document).ready(function () {
    var length = $('#EmployeeExistingSelect').children('option').length;
    if (length <= 0) {
        $("#divExist").attr("hidden", true);
    }
});
