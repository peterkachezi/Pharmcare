
//Show Modal.
function addNewOrder() {
    $("#ModalCreatePrescription").modal();
}
//Add Multiple Order.
$("#addToList").click(function (e) {

    e.preventDefault();

    if ($.trim($('#txtMedicineName').val()) == "" || $.trim($('#txtMedicineName').val()) == null) {
        $('#txtMedicineName').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please select Medicine",
            showConfirmButton: true,
        });

        return false;
    }

    if ($.trim($('#txtFrequency').val()) == "" || $.trim($('#txtFrequency').val()) == null) {
        $('#txtFrequency').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please select Frequency",
            showConfirmButton: true,
        });

        return false;
    }

    if ($.trim($('#txtNoOfDays').val()) == "" || $.trim($('#txtNoOfDays').val()) == null) {
        $('#txtNoOfDays').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter No of Days",
            showConfirmButton: true,
        });

        return false;
    }

    if ($.trim($('#txtWhenTotake').val()) == "" || $.trim($('#txtWhenTotake').val()) == null) {
        $('#txtWhenTotake').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please select When to take",
            showConfirmButton: true,
        });

        return false;
    }

    if ($.trim($('#txtQuantity').val()) == "" || $.trim($('#txtQuantity').val()) == null) {
        $('#txtWhenTotake').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Quantity is required field",
            showConfirmButton: true,
        });

        return false;
    }



    var MedicineId = $("#txtMedicineId").val();

    var MedicineName = $("#txtMedicineName").val();

    var Frequency = $("#txtFrequency").val();

    var NoOfDays = $("#txtNoOfDays").val();

    var WhenTotake = $("#txtWhenTotake").val();

    var Quantity = $("#txtQuantity").val();

    var SellingPrice = $("#txtSellingPrice").val();

    var detailsTableBody = $("#detailsTable tbody");

    var productItem =
        '<tr><td style="display:none;">' + MedicineId +
        '</td><td>' + MedicineName +
        '</td><td>' + Frequency +
        '</td><td>' + NoOfDays +
        '</td><td>' + WhenTotake +
        '</td><td>' + Quantity +
        '</td><td>' + SellingPrice +
        '</td><td><a data-itemId="0" href="#" class="deleteItem">Remove</a></td></tr>';



    detailsTableBody.append(productItem);

    console.log(productItem);

    clearItem();
});
//After Add A New Order In The List, Clear Clean The Form For Add More Order.
function clearItem() {

    $("#txtMedicineId").val('');

    $("#txtFrequency").val('');

    $("#txtNoOfDays").val('');

    $("#txtWhenTotake").val('');

    $("#txtQuantity").val('');
}
// After Add A New Order In The List, If You Want, You Can Remove It.
$(document).on('click', 'a.deleteItem', function (e) {
    e.preventDefault();
    var $self = $(this);
    if ($(this).attr('data-itemId') == "0") {
        $(this).parents('tr').css("background-color", "#ff6347").fadeOut(800, function () {
            $(this).remove();
        });
    }
});
//After Click Save Button Pass All Data View To Controller For Save Database
function saveOrder(data) {
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "/Admin/Prescriptions/CreatePrescriptions/",
        data: data,
        success: function (result) {
            alert(result);
            location.reload();
        },
        error: function () {
            alert("Error!")
        }
    });
}
//Collect Multiple Order List For Pass To Controller
$("#saveOrder").click(function (e) {


    if ($.trim($('#txtTreatmentFor').val()) == "" || $.trim($('#txtTreatmentFor').val()) == null) {
        $('#txtTreatmentFor').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Treatment For is a required field",
            showConfirmButton: true,
        });

        return false;
    }



    $("#ModalCreatePrescription").modal('hide');

    e.preventDefault();

    var orderArr = [];

    console.log(orderArr);

    orderArr.length = 0;

    $.each($("#detailsTable tbody tr"), function () {

        orderArr.push({

            MedicineId: $(this).find('td:eq(0)').html(),

            MedicineName: $(this).find('td:eq(1)').html(),

            Frequency: $(this).find('td:eq(2)').html(),

            NoOfDays: $(this).find('td:eq(3)').html(),

            WhenTotake: $(this).find('td:eq(4)').html(),

            Quantity: $(this).find('td:eq(5)').html(),

            SellingPrice: $(this).find('td:eq(6)').html()
        });
    });


    var things = ({

        Note: $("#txtNote").val(),

        TreatmentFor: $("#txtTreatmentFor").val(),

        PatientId: $("#txtpatientId2").val(),

        PrescriptionDetailDTO: orderArr
    });

    console.log(things);

    $(document).ready(function () {

        var employeeList = things;

        var PrescriptionDTO = {

            prescriptionDTO: employeeList

        }

        $.ajax({

            dataType: 'json',

            type: 'POST',

            url: '/Admin/Prescriptions/Create/',

            data: PrescriptionDTO,

            success: function (response) {
                if (response.success) {

                    swal({
                        position: 'top-end',

                        type: "success",

                        title: response.responseText,

                        showConfirmButton: false,

                    }), setTimeout(function () { window.location = "/Admin/Prescriptions/"; }, 2000);



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


            failure: function (response) {

                $('#InfoPanel').html(response);

            }
        });
    });



    //$.when(saveOrder(data)).then(function (response) {
    //    console.log(response);
    //}).fail(function (err) {
    //    console.log(err);
    //});
});
function GetMedicineId() {

    var idNumber = $('#txtMedicineId').val();

    var id = idNumber;

    $.get("/Admin/Medicines/GetById/?Id=" + id, function (data, status) {

        if (data == null) {
            alert("Does not exist");
        } else {

            console.log(data.data);

            $("#txtMedicineName").val(data.data.medicineFullName);
            $("#txtSellingPrice").val(data.data.sellingPrice);

        }

    });
};
function Details(id, patientId) {

    window.location.href = "/Admin/Prescriptions/Details?Id=" + id + "&PatientId=" + patientId;

}
function DowloadPrescripton(id, patientId) {

    window.location.href = "/Admin/Prescriptions/DowloadPrescripton/?Id=" + id + "&PatientId=" + patientId;


}
function IssueMedicine(e) {


    var id = e;

    $.get("/Admin/Prescriptions/GetByPrescriptionDetailId/?Id=" + id, function (data, status) {

        console.log(data);

        if (data == null) {

            alert("Does not exist");

        } else {

            $("#txtMedicineId").val(data.data.id);

            $("#txtPrescriptionId").val(data.data.prescriptionId);

            $("#txtName").val(data.data.medicineFullName);

            $("#txtShelfName").val(data.data.shelfName);

            $("#txtManufacturerPrice").val(data.data.manufacturerPrice);

            $("#txtSellingPrice").val(data.data.sellingPrice);

            $("#txtCurrentStock").val(data.data.quantity);

            $('#ModalIssuedMedicine').modal({ backdrop: 'static', keyboard: false })

            $("#ModalIssuedMedicine").modal('show');
        }

    });


};
$("#btnSubmit").click(function () {

    if ($('#txtQuantity').val() == '') {
        $('#txtQuantity').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Quantity",
            showConfirmButton: true,
        });

        return false;
    }

    $("#ModalIssuedMedicine").modal('hide');

    $("#divLoader").show();


    var formData = new FormData($('#frmIssuedMedicine').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Prescriptions/IssueMedicine", // NB: Use the correct action name
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

                }), setTimeout(function () { location.reload(); }, 1500);

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
function IssueMedicine1(e) {

    $("#divLoader").show();

    var id = e;

    console.log(id);

    swal(

        {
            title: "Are you sure?",

            //text: "Once deleted, you will not be able to recover this  file!",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $.ajax({

                type: "GET",

                url: "/Admin/Prescriptions/IssueMedicine/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 1500);

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
function UnDoIssueMedicine(e) {

    $("#divLoader").show();
    var id = e;

    console.log(id);

    swal(

        {
            title: "Are you sure?",

            //text: "Once deleted, you will not be able to recover this  file!",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $.ajax({

                type: "GET",

                url: "/Admin/Prescriptions/UnDoIssueMedicine/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 1500);

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

            $("#divLoader").hide();
        }
    );
}
function GenerateInvoice(id, patientId) {
    window.location.href = "/Admin/Invoices/GenerateInvoice/?Id=" + id + "&PatientId=" + patientId;
}
function DowloadInvoice(id, patientId) {
    window.location.href = "/Admin/Invoices/DowloadInvoice/?Id=" + id + "&PatientId=" + patientId;
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

                url: "/Admin/Prescriptions/Delete/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 1500);

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
function DeleteDetails(e) {

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

                url: "/Admin/Prescriptions/DeleteDetails/" + id,

                success: function (response) {

                    if (response.success) {

                        swal({

                            position: 'top-end',

                            type: "success",

                            title: response.responseText,

                            showConfirmButton: false,

                            // timer: 2000,

                        });
                        setTimeout(function () { location.reload(); }, 1500);

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
function SearchMember(e, p) {

    $("#divLoader").show();

    debugger

    console.log(dateFrom);

    window.location.href = "/Admin/Prescriptions/Details?Id=" + e + "&PatientId=" + p;

}
function CalculateBalance() {

    //var FinalAmount = $("#txtTotalAmountPayment").val();
    //var PaymentAmount = $("#txtPaymentAmount").val();
    //var BalanceAmount = PaymentAmount - FinalAmount;

    var sum1 = parseFloat($('input[name=AmountPayable]').val());

    var sum2 = parseFloat($('input[name=AmountPaid]').val());

    $('#txtBalance').val(sum2 - sum1);

}
$("#btnPayInvoice").click(function () {

    if ($('#txtPaymentAmount').val() == '') {
        $('#txtPaymentAmount').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Payment amount",
            showConfirmButton: true,
        });

        return false;
    }
    $("#PaymentModal").modal('hide');

    $("#divLoader").show();


    var formData = new FormData($('#frmPayInvoice').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/Invoices/CreatePayment", // NB: Use the correct action name
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

                }), setTimeout(function () { location.reload(); }, 1500);

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
function SaveStudent() {

    $("#PaymentModal").modal('hide');

    $("#divLoader").show();

    let formData = {
        AmountPayable: $("#txtAmountPayable").val(),
        PaymentMode: $("#txtPaymentMode").val(),
        AmountPaid: $("#txtPaymentAmount").val(),
        Balance: $("#txtBalance").val(),
        PrescriptionId: $("#txtPrescriptionId").val(),

    }
    console.log(formData)
    $.ajax({
        url: "/Admin/Invoices/CreatePayment",
        type: "POST",
        data: formData,

        success: function (response) {

            if (response.success) {

                swal({
                    position: 'top-end',

                    type: "success",

                    title: response.responseText,

                    showConfirmButton: false,

                }), setTimeout(function () { window.location = "/Admin/Prescriptions/"; }, 2000);

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

        error: function (request, status, error) {

            alert(request.responseText);
        }
    });
}