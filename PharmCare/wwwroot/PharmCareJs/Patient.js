
$(document).ready(function () {


    $("#btnSearch").click(function () {

        var phoneNumber = $("#txtSearchPhoneNumber").val();

        var setData = $("#tblFilterPatient");

        setData.html("");

        $.ajax({

            type: "post",
            url: "/Admin/Patients/GetByPhoneNumber?PhoneNumber=" + phoneNumber,
            contentType: "html",
            success: function (result) {

                if (result.length == 0) {

                    setData.append('<tr style="color:red"> <td colspan="3">No Match Data </td> </tr>')
                }

                else {

                    $.each(result, function (index, value) {

                        var Data = "<tr>" +

                            "<td>" + value.patientNumber + "</td>" +

                            "<td>" + value.fullName + "</td>" +

                            "<td>" + value.phoneNumber + "</td>" +

                            "<td>" + value.dateOfBirth + "</td>" +

                            "<td>" + value.age + "</td>" +

                            "<td>" + value.height + "</td>" +

                            "<td>" + value.weight + "</td>" +

                            "<td>" + value.residence + "</td>" +

                            "<td>" + value.createDate + "</td>" +

                            "<td><a class='btn-success  btn-sm' href='' onclick ='GetRecord('" + value.id + "')'  > <i class='fa fa-edit'> Edit</i></a > <a class=' btn-danger  btn-sm' href='#' onclick='DeleteRecord('" + value.id + "')' ><i class='fa fa-trash'> Delete</i></a>"

                        "</td>" +


                            "</tr>";

                        setData.append(Data);

                        console.log(result);
                    });
                }
            }
        });

    });

});

function GetAllData() {

    var t = $('#tblCustomers').DataTable({
        "ajax": {
            "url": "/Admin/Patients/GetCustomers",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [

            { "data": "id" },
            { "data": "name" },
            { "data": "newCreateDate" },
            { "data": "createdByName" },

            {
                data: null,
                mRender: function (data, type, row) {
                    return "<a href='#' class='btn-sm success' onclick=GetRecord('" + row.id + "'); >Edit</a> / <a href='#' class='btn-sm danger' onclick=DeleteRecord('" + row.id + "'); > Delete</a > ";

                }
            }

        ]

    });
    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}

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

                url: "/Admin/Patients/Delete/" + id,

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

$("#btnSubmitPatient").click(function () {


    if ($('#txtFirstName').val() == '') {
        $('#txtFirstName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter first name",
            showConfirmButton: true,
        });

        return false;
    }

    if ($('#txtLastName').val() == '') {
        $('#txtLastName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter last name",
            showConfirmButton: true,
        });

        return false;
    }



    if ($('#txtDateOfBirth').val() == '') {
        $('#txtDateOfBirth').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter date Of birth",
            showConfirmButton: true,
        });

        return false;
    }

    $("#ModalCreatePatient").modal('hide');

    $("#divLoader").show();


    var formData = new FormData($('#frmAddPatients').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Patients/Create", // NB: Use the correct action name
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

$("#btnUpdatePatient").click(function () {


    if ($('#txtFirstName1').val() == '') {
        $('#txtFirstName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter first name",
            showConfirmButton: true,
        });

        return false;
    }

    if ($('#txtLastName1').val() == '') {
        $('#txtLastName1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter last name",
            showConfirmButton: true,
        });

        return false;
    }

    if ($('#txtDateOfBirth1').val() == '') {
        $('#txtDateOfBirth1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter date Of birth",
            showConfirmButton: true,
        });

        return false;
    }

    $("#ModalUpdatePatient").modal('hide');

    $("#divLoader").show();


    var formData = new FormData($('#frmUpdatePatient').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Patients/Update", // NB: Use the correct action name
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

function GetRecord(e) {

    var id = e;

    $.get("/Admin/Patients/GetById/?Id=" + id, function (data, status) {

        console.log(data);
        if (data == null) {
            alert("Does not exist");
        } else {

            $("#txtId").val(data.data.id);
            $("#txtFirstName1").val(data.data.firstName);
            $("#txtLastName1").val(data.data.lastName);
            $("#txtEmail1").val(data.data.email);
            $("#txtPhoneNumber1").val(data.data.phoneNumber);
            $("#txtDateOfBirth1").val(data.data.dateOfBirth);
            $("#txtGender1").val(data.data.gender);
            $("#txtHeight1").val(data.data.height);
            $("#txtResidence1").val(data.data.residence);
            $("#txtWeight1").val(data.data.weight);


            $('#ModalUpdatePatient').modal({ backdrop: 'static', keyboard: false })
            $("#ModalUpdatePatient").modal('show');
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


function GetPatientRecord(e) {

    var id = e;

    $.get("/Admin/Patients/GetById/?Id=" + id, function (data, status) {

        console.log(data);

        if (data == null) {

            alert("Does not exist");

        } else {

            $("#txtpatientId2").val(data.data.id);
         

            $('#ModalCreatePrescription').modal({ backdrop: 'static', keyboard: false })
            $("#ModalCreatePrescription").modal('show');
        }

    });
};
