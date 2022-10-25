
function DeleteRecord(e) {

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

                url: "/Admin/Suppliers/Delete/" + id,

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

$("#btnSubmit").click(function () {


    if ($('#txtName').val() == '') {
        $('#txtName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's name",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtEmail').val() == '') {
        $('#txtEmail').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's email",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtPhoneNumber').val() == '') {
        $('#txtPhoneNumber').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's phone number",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtTown').val() == '') {
        $('#txtTown').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's town",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtFirstName').val() == '') {
        $('#txtFirstName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter contact person's first name",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtLastName').val() == '') {
        $('#txtLastName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter contact person's last name",
            showConfirmButton: true,
        });
        return false;
    }

    $("#ModalCreateSupplier").modal('hide');

    $("#divLoader").show();


    var formData = new FormData($('#frmCreateSupplier').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Suppliers/Create", // NB: Use the correct action name
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,


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

            $("#divLoader").hide();
        },


        error: function (error) {
            alert("errror");
        }
    });

});

$("#btnUpdate").click(function () {

    if ($('#txtName1').val() == '') {
        $('#txtName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's name",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtEmail1').val() == '') {
        $('#txtEmail1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's email",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtPhoneNumber1').val() == '') {
        $('#txtPhoneNumber1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's phone number",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtTown1').val() == '') {
        $('#txtTown1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter manufacturer's town",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtFirstName1').val() == '') {
        $('#txtFirstName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter contact person's first name",
            showConfirmButton: true,
        });
        return false;
    }

    if ($('#txtLastName1').val() == '') {
        $('#txtLastName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter contact person's last name",
            showConfirmButton: true,
        });
        return false;
    }

  

    $("#ModalUpdateSupplier").modal('hide');

    $("#divLoader").show();      

    var formData = new FormData($('#frmUpdateSupplier').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Suppliers/Update", // NB: Use the correct action name
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,

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

            $("#divLoader").hide();
        }
    });

});

function GetRecord(e) {

    var id = e;

    $("#divLoader").show();

    $.get("/Admin/Suppliers/GetById/?Id=" + id, function (data, status) {

        console.log(data);
        if (data == null) {
            alert("Does not exist");
        } else {

            $("#txtId").val(data.data.id);

            $("#txtContactId").val(data.data.contactId);

            $("#txtContactFirstName1").val(data.data.contactFirstName);

            $("#txtContactLastName1").val(data.data.contactLastName);

            $("#txtContactPhoneNumber1").val(data.data.contactPhoneNumber);

            $("#txtContactEmail1").val(data.data.contactEmail);

            $("#txtContactCountryId1").val(data.data.contactCountryId);

            $("#txtName1").val(data.data.name);

            $("#txtEmail1").val(data.data.email);

            $("#txtPhoneNumber1").val(data.data.phoneNumber);

            $("#txtTown1").val(data.data.town);

            $("#txtCountryId1").val(data.data.countryId);

            $("#txtProductTypeId1").val(data.data.productTypeId);

            $("#txtPhysicalAddress1").val(data.data.physicalAddress);

            $("#divLoader").hide();

            $('#ModalUpdateSupplier').modal({ backdrop: 'static', keyboard: false })

            $("#ModalUpdateSupplier").modal('show');
        }

    });
};


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


