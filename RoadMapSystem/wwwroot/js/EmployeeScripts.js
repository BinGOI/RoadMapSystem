
$(document).ready(function () {
    $("#AddExistButton").bind('click',
        function ClickExistingUser(mentorLogin) {
            $ajax({
                url: "Employees/AddExistingIntern",
                data: {
                    MentorLogin: mentorLogin,
                    InternId: $("#EmployeeExistingSelect").value
                },
                success: function () {
                    window.location.href = "Employees/Info/" + mentorLogin
                }
            });
        })
});