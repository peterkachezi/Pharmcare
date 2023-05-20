
$(document).ready(function () {

});


function DeleteExpiredDrugs(e) {

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

                url: "/Admin/StockManager/DeleteExpiredDrugs/?BatchNo=" + id,

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

                url: "/Admin/Medicines/Delete/" + id,

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
            title: "Please enter medicine name",
            showConfirmButton: true,
        });

        return false;
    }


    if ($('#txtStrength').val() == '') {
        $('#txtStrength').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Strength",
            showConfirmButton: true,
        });

        return false;
    }

   
    if ($('#txtManufacturerPrice').val() == '') {
        $('#txtManufacturerPrice').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Cost Price",
            showConfirmButton: true,
        });

        return false;
    }


    if ($('#txtSellingPrice').val() == '') {
        $('#txtSellingPrice').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Selling Price",
            showConfirmButton: true,
        });

        return false;
    }



    $("#ModalCreateMedicine").modal('hide');

    $("#divLoader").show();

    var formData = new FormData($('#frmCreateMedicine').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Medicines/Create", // NB: Use the correct action name
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
            title: "Please enter Medicines Name",
            showConfirmButton: true,
        });

        return false;
    }



    if ($('#txtStrength1').val() == '') {
        $('#txtStrength1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Strength",
            showConfirmButton: true,
        });

        return false;
    }

    

    if ($('#txtManufacturerPrice1').val() == '') {
        $('#txtManufacturerPrice1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Cost Price",
            showConfirmButton: true,
        });

        return false;
    }

    if ($('#txtSellingPrice1').val() == '') {
        $('#txtSellingPrice1').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Selling Price",
            showConfirmButton: true,
        });

        return false;
    }

    
    $("#ModalUdateMedicine").modal('hide');

    $("#divLoader").show();

    var formData = new FormData($('#frmUpdateMedicine').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Medicines/Update", // NB: Use the correct action name
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

    $.get("/Admin/Medicines/GetById/?Id=" + id, function (data, status) {

        console.log(data);

        if (data == null) {

            alert("Does not exist");

        } else {

            $("#txtId1").val(data.data.id);

            $("#txtName1").val(data.data.name);

            $("#txtGenericName1").val(data.data.genericName);

            $("#txtUnitId1").val(data.data.unitId);

            $("#txtShelfId1").val(data.data.shelfId);

            $("#txtCategoryId1").val(data.data.categoryId);

            $("#txtManufacturerPrice1").val(data.data.manufacturerPrice);

            $("#txtSellingPrice1").val(data.data.sellingPrice); 

            $("#txtStatus1").val(data.data.status);

            $("#txtSupplierId1").val(data.data.SupplierId);

            $("#txtDescription1").val(data.data.description);

            $("#txtBarCode1").val(data.data.barCode);

            $("#txtMedicalConditionId1").val(data.data.medicalConditionId);

            $("#txtUnitId1").val(data.data.unitId);


            $('#ModalUdateMedicine').modal({ backdrop: 'static', keyboard: false })

            $("#ModalUdateMedicine").modal('show');
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


