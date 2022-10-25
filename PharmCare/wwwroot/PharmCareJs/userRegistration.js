﻿
function ShowLoader() {

    $("#divLoader").show();
}

function HideLoader() {

    $("#divLoader").hide();
}

$("#btnCreateAccount").click(function () {

    if ($('#txtFirstName').val() == '') {
        $('#txtFirstName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "first name is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtLastName').val() == '') {
        $('#txtLastName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "last name is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtPhoneNumber').val() == '') {
        $('#txtPhoneNumber').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "phone number is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtEmail').val() == '') {
        $('#txtEmail').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Email is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    ShowLoader();


    $("#modalRegisterUser").modal('hide');

    var data = $("#frmRegisterUser").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/UserManager/RegisterUser/",

        data: data,

        beforeSend: function () { ShowLoader(); },

        success: function (response) {

            if (response.success) {
                swal({
                    position: 'top-end',
                    type: "success",
                    title: response.responseText,
                    showConfirmButton: false,
                }), setTimeout(function () { location.reload(); }, 3000);

            } else {
                swal({
                    position: 'top-end',
                    type: "error",
                    title: response.responseText,
                    showConfirmButton: true,
                    timer: 5000,
                });
            }
        },

        error: function (response) {
            alert("error!");
        },
        complete: function () {
            HideLoader();
        }
    })

})

$("#btnUpdateAccount").click(function () {

    if ($('#txtfirstName1').val() == '') {
        $('#txtfirstName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "first name is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtlastName1').val() == '') {
        $('#txtlastName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "last name is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    if ($('#txtPhoneNumber1').val() == '') {
        $('#txtPhoneNumber1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "phone number is a required field",
            showConfirmButton: true,
        });
        return false;
    }


    if ($('#txtEmail1').val() == '') {
        $('#txtEmail1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Email is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtRoleName1').val() == '') {
        $('#txtRoleName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Role Name is a required field",
            showConfirmButton: true,
        });
        return false;
    }

    $("#modalUpdateUser").modal('hide');

    var data = $("#frmUpdateAccount").serialize();

    $.ajax({

        type: "POST",

        url: "/Admin/UserManager/Update/",

        data: data,

        beforeSend: function () { ShowLoader(); },

        success: function (response) {

            if (response.success) {
                swal({
                    position: 'top-end',
                    type: "success",
                    title: response.responseText,
                    showConfirmButton: false,
                }), setTimeout(function () { location.reload(); }, 3000);

            } else {
                swal({
                    position: 'top-end',
                    type: "error",
                    title: response.responseText,
                    showConfirmButton: true,
                    timer: 5000,
                });
            }
        },

        error: function (response) {
            alert("error!");
        },
        complete: function () {
            HideLoader();
        }
    })

})

function GetUser(e) {

    var id = e;

    console.log(id);

    $.get("/Admin/UserManager/GetUserById/?Id=" + id, function (data, status) {
        console.log(data);
        if (data == null) {
            alert("Does not exist");
        } else {

            $("#txtId").val(data.data.id);
            $("#txtfirstName1").val(data.data.firstName);
            $("#txtlastName1").val(data.data.lastName);
            $("#txtphoneNumber1").val(data.data.phoneNumber);
            $("#txtemail1").val(data.data.email);
            
          
            $("#txtroleName1").val(data.data.roleName);


            $('#modalUpdateUser').modal({ backdrop: 'static', keyboard: false })
            $("#modalUpdateUser").modal('show');
        }

    });
};

function ViewDetail(e) {

    var id = e;

    window.location.href = "/Admin/UserManager/ViewDetails/" + id;

}

function GetBranchesEdit() {

    var Id = $('#txtRegionId1').val();



    $.ajax({
        type: "GET",
        url: "/Admin/Branches/GetBranches/" + Id,
        data: "{ }",

        success: function (data) {
            var s = '<option value="-1">Please Select a Model</option>';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].makeId + '">' + data[i].makeName + '</option>';
            }
            $("#departmentsDropdown").html(s);

            console.log(data);
        }


    });
}
//Allow users to enter numbers only
$(".numericOnly").bind('keypress', function (e) {
    if (e.keyCode == '9' || e.keyCode == '16') {
        return;
    }
    var code;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    if (e.which == 46)
        return false;
    if (code == 8 || code == 46)
        return true;
    if (code < 48 || code > 57)
        return false;
});

//Disable paste
$(".numericOnly").bind("paste", function (e) {
    e.preventDefault();
});

$(".numericOnly").bind('mouseenter', function (e) {
    var val = $(this).val();
    if (val != '0') {
        val = val.replace(/[^0-9]+/g, "")
        $(this).val(val);
    }
});

function DeactivateAccount(e) {

    var id = e;

    swal(

        {
            title: "Are you sure?",

            text: "Disabling user account!",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $.ajax({

                type: "GET",

                url: "/Admin/UserManager/DeactivateAccount/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 3000);

                    }

                    else {
                        swal({
                            position: 'top-end',
                            type: "error",
                            title: response.responseText,
                            showConfirmButton: true,
                            timer: 5000,
                        });

                    }

                },
                error: function (response) {

                    console.log(response);
                    swal({
                        position: 'top-end',
                        type: "error",
                        title: "Server error ,kindly contact the admin for assistance",
                        showConfirmButton: false,
                        timer: 5000,
                    });

                }

            })

        });
}

function EnableAccount(e) {

    var id = e;

    swal(

        {
            title: "Are you sure?",

            text: "Enabling user account!",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $.ajax({

                type: "GET",

                url: "/Admin/UserManager/ActivateAccount/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 3000);

                    }

                    else {
                        swal({
                            position: 'top-end',
                            type: "error",
                            title: response.responseText,
                            showConfirmButton: true,
                            timer: 5000,
                        });

                    }

                },
                error: function (response) {

                    console.log(response);
                    swal({
                        position: 'top-end',
                        type: "error",
                        title: "Server error ,kindly contact the admin for assistance",
                        showConfirmButton: false,
                        timer: 5000,
                    });

                }

            })

        });
}

function DeleteUser(e) {

    $("#divLoader").show();
    var id = e;

    console.log(id);

    swal(

        {
            title: "Are you sure?",

            text: "Once deleted, you will not be able to recover this  file!",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $.ajax({

                type: "GET",

                url: "/Admin/UserManager/Delete/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 3000);

                    }

                    else {
                        swal({
                            position: 'top-end',
                            type: "error",
                            title: response.responseText,
                            showConfirmButton: true,
                            timer: 5000,
                        });
                        $("#divLoader").hide();
                    }

                },
                error: function (response) {
                    swal({
                        position: 'top-end',
                        type: "error",
                        title: "Server error ,kindly contact the admin for assistance",
                        showConfirmButton: false,
                        timer: 5000,
                    });
                    $("#divLoader").hide();
                }

            })

        }
    );
}

