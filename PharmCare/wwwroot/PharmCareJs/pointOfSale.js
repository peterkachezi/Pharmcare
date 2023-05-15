
$(document).ready(function () {

    $("input[type=text]").keyup(function () {
        CalculateBalance();
    });

    $("#btnAddToList").click(function () {

        AddToTheList();
    });

    $("#btnPayment").click(function () {
        SaveTransaction();
    });

    hidshowbtncheckout();

    var DefaultVal = 0;

    $("#RowsCount").text(DefaultVal);
});

function GetMedicines() {

    var SupplierId = $('#txtSupplierId').val();

    console.log(SupplierId);

    $.ajax({
        type: "GET",
        url: "/Admin/StockManager/GetMedicine/" + SupplierId,
        data: "{ }",

        success: function (data) {

            var s = '<option value="-1">Select medicine</option>';

            for (var i = 0; i < data.length; i++) {

                s += '<option value="' + data[i].medicineId + '">' + data[i].medicineName + '</option>';

            }
            $("#medicineDropdown").html(s);

            console.log(data);
        }
    });
}

function GetMedicineDetailsData() {

    var k = $('#medicineDropdown').val();

    console.log(k);

    if ($('#medicineDropdown').val() == null) {

        $('#medicineDropdown').focus();

        swal({
            position: 'top-end',
            type: "error",
            title: "Please select medicine",
            showConfirmButton: true,
        });

        return false;
    }

    var medicineId = $('#medicineDropdown').val();

    console.log(medicineId);

    $("#divLoader").show();


    $.get("/Admin/StockManager/GetByMedicineId/?Id=" + medicineId,

        function (data, status) {

            console.log(data);

            if (data.data == false) {

                swal({
                    position: 'top-end',
                    type: "error",
                    title: "This product is out stock",
                    showConfirmButton: true,
                });

            } else {

                $("#divLoader").hide();


                $("#txtMedicineId").val(data.data.id);

                $("#txtMedicineName").val(data.data.name);

                $("#txtSellingPrice").val(data.data.sellingPrice);

                $("#txtCurrentQuantity").val(data.data.quantity);
            }

        });
}

function GetPaymentType() {

    if ($('#txtBankType').val() == '2') {

        $('#lblBank').show();

        $('#bank_div').show();

    } else {

        $('#lblBank').hide();
        $('#bank_div').hide();
    }
}

$("#btnPrint").click(function () {

    $("#ReceiptModal").modal('hide');

    $("#divLoader").show();

    var receiptNo = $('#txtReceiptNo').val();

    console.log(receiptNo); 

    var url = "/Admin/PointOfSale/GetReceipt?ReceiptNo=" + receiptNo;
    ResetItem();
    RemoveEverything();
    //window.open(window.location.href = url, '_blank');

    window.location.href = url;

    $("#divLoader").hide();
})


function RemoveEverything() {

    debugger

    var table = $('#tblProducts').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();

    CalculateSubTotal();

    FinaItemTotal();

    hidshowbtncheckout();

    RemoveCount();
}





$("#btnPrint1").click(function () {

    $("#ReceiptModal").modal('hide');

    $("#divLoader").show();

    var formData = new FormData($('#frmPrintReceipt').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/PointOfSale/GetReceipt", // NB: Use the correct action name
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

$("#btnCancel").click(function () {
    setTimeout(function () { location.reload() });
})

function hidshowbtncheckout() {

    if (($.trim($("tbody").html()) == ""))

        $("#btnCheckout").hide();

    else
        $("#btnCheckout").show();

}
function PaymentWindow() {

    if ($('#txtTotalAmount').text() < 1) {
        $('#txtTotalAmount').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please add items to list",
            showConfirmButton: true,
        });

        $("#divLoader").hide();

        return false;
    }

    $("#txtBalance").val("0.00");

    $('#PaymentModal').modal({ backdrop: 'static', keyboard: false })

    $("#PaymentModal").modal('show');

}

function SaveTransaction() {

    if ($('#txtPaymentAmount').val() < 1) {
        $('#txtPaymentAmount').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter the amount paid",
            showConfirmButton: true,
        });
        return false;
    }
    var amountPaid = parseFloat($("#txtPaymentAmount").val());

    var totalAmountPayment = parseFloat($("#txtTotalAmountPayment").val());

    if (amountPaid < totalAmountPayment) {
        swal({
            position: 'top-end',
            type: "error",
            title: "You have entered insufficient funds ",
            showConfirmButton: true,
        });

        return false;
    }



    $("#divLoader").show();


    $("#PaymentModal").modal('hide');

    var SalesDTO = {};

    ListOfSalesDetails = new Array();

    SalesDTO.TotalAmount = $("#txtTotalAmountPayment").val();

    SalesDTO.AmountPaid = $("#txtPaymentAmount").val();

    SalesDTO.Balance = $("#txtBalance").val();


    $("#tblProducts").find("tr:gt(0)").each(function () {

        var SalesDetailsDTO = {};

        SalesDetailsDTO.Total = parseFloat($(this).find("td:eq(5)").text());

        SalesDetailsDTO.MedicineId = $(this).find("td:eq(0)").text();

        SalesDetailsDTO.SellingPrice = parseFloat($(this).find("td:eq(2)").text());

        SalesDetailsDTO.Quantity = parseFloat($(this).find("td:eq(3)").text());

        ListOfSalesDetails.push(SalesDetailsDTO);

    });

    SalesDTO.ListOfSalesDetails = ListOfSalesDetails;

    console.log(SalesDTO);

    $.ajax({

        type: "POST",
        url: "/Admin/PointOfSale/SaveTransaction/",
        data: SalesDTO,

        success: function (response) {

            if (response.success) {

                console.log(response);

                var grnNumber = response.responseText;

                $("#txtReceiptNo").val(grnNumber);

                $('#ReceiptModal').modal({ backdrop: 'static', keyboard: false })

                $("#ReceiptModal").modal('show');


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



        error: function (response) {
            alert("error!");

        }
    })

}

function AddToTheList() {




    if ($('#txtQuantity').val() == '' || $('#txtQuantity').val() == '0.00' || $('#txtQuantity').val() == '0') {
        $('#txtQuantity').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter quantity",
            showConfirmButton: true,
        });

        return false;
    }

    if ($('#txtSellingPrice').val() == '' || $('#txtSellingPrice').val() == '0.00' || $('#txtSellingPrice').val() == '0') {
        $('#txtSellingPrice').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please search medicine before adding to list",
            showConfirmButton: true,
        });

        return false;
    }


    if ($('#txtSellingPrice').val() < 1) {
        $('#txtSellingPrice').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please search medicine before adding to list",
            showConfirmButton: true,
        });

        return false;
    }


    if ($('#txtCurrentQuantity').val() < 1) {
        $('#txtCurrentQuantity').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "This medicine is currently out of stock",
            showConfirmButton: true,
        });

        return false;
    }


    var tblItemList = $("#tblProducts");

    var SellingPrice = parseFloat($("[id*=txtSellingPrice]").val());

    var Quantity = parseFloat($("[id*=txtQuantity]").val());

    var MedicineId = $("[id*=txtMedicineId]").val()

    var ItemName = $("#txtMedicineName").val();

    var Total = parseFloat(SellingPrice * Quantity);

    console.log(MedicineId)

    var rowCount = $("#tblProducts tr").length;

    var ItemList =

        "<tr><td hidden>" + MedicineId +

        "</td><td>" + ItemName +

        "</td><td>" + parseFloat(SellingPrice).toFixed(2) +

        "</td><td>" + parseFloat(Quantity).toFixed(2) +

        "</td><td>" + parseFloat(Total).toFixed(2) +

        "</td><td><a href='#' class='btn-danger  btn-sm' name='Remove' onclick=RemoveItem(this) >  <i class='fa fa-trash'> Remove</i> </a>  </td></tr>";


    tblItemList.append(ItemList);


    FinaItemTotal();

    ResetItem();

    hidshowbtncheckout();

    AddCount();
}

function AddCount() {

    var value = parseFloat($("[id*=RowsCount]").text());

    var sum = parseInt(1) + parseInt(value);

    $("#RowsCount").text(sum);
}

function RemoveCount() {

    var value = parseFloat($("[id*=RowsCount]").text());

    var remove = parseInt(value) - parseInt(1);

    $("#RowsCount").text(remove);
}

function RemoveItem(Id) {

    swal(

        {
            title: "Are you sure you wish to remove this entry?",

            type: "success",

            showCancelButton: true,

            confirmButtonColor: "##62b76e",

            confirmButtonText: "Yes, Procceed!",

            closeOnConfirm: false
        },

        function () {

            $(Id).closest('tr').remove();

            CalculateSubTotal();

            FinaItemTotal();

            hidshowbtncheckout();

            RemoveCount();

            swal({

                position: 'top-end',

                type: "success",

                title: "Record has been successfully removed",

                showConfirmButton: false,

                timer: 2000,

            });
        }
    );










}

function ResetItem() {

    $("#txtQuantity").val('0');

    $("#txtCurrentQuantity").val('0');

    $("#txtTotal").val('0.00');

    $("#txtSellingPrice").val('0');
}

function FinaItemTotal() {

    $("#txtTotalAmount").text("0.00");

    var TotalAmount = 0.00;

    $("#tblProducts").find("tr:gt(0)").each(function () {

        var Total = parseFloat($(this).find("td:eq(4)").text());

        TotalAmount += Total;
    });

    $("#txtTotalAmount").text(parseFloat(TotalAmount).toFixed(2));

    $("#txtTotalAmountPayment").val(parseFloat(TotalAmount).toFixed(2));

    $("#txtPaymentTotal").val(parseFloat(TotalAmount).toFixed(2));

}

function CalculateBalance() {



    var FinalAmount = $("#txtTotalAmountPayment").val();

    var PaymentAmount = $("#txtPaymentAmount").val();

    var BalanceAmount = parseFloat(PaymentAmount) - parseFloat(FinalAmount);

    $("#txtBalance").val(parseFloat(BalanceAmount).toFixed(2));

    if (PaymentAmount == '' || PaymentAmount == "") {

        $("#txtBalance").val("0.00");
    }

    if (parseFloat(BalanceAmount) == 0) {

        $("#btnPayment").removeAttr("disabled");

    } else {

        $("#btnPayment").attr("disabled", "disabled");
    }
}

function CalculateSubTotal() {

    var SellingPrice = parseFloat($("[id*=txtSellingPrice]").val());

    var Quantity = parseFloat($("[id*=txtQuantity]").val());

    var total = parseFloat(SellingPrice * Quantity);

    $("#txtTotal").val(parseFloat(total).toFixed(2));

}

function GetMedForStock(e) {

    var id = e;

    $.get("/Admin/StockManager/GetByMedicineId/?Id=" + id, function (data, status) {

        console.log(data);

        if (data == null) {

            alert("Does not exist");

        } else {

            $("#txtId1").val(data.data.id);

            $("#txtName1").val(data.data.name);

            $("#txtGenericName1").val(data.data.genericName);

            $("#txtManufacturerPrice1").val(data.data.manufacturerPrice);

            $("#txtSellingPrice1").val(data.data.sellingPrice);

            $("#txtCurrentStock").val(data.data.quantity);

            $("#txtSupplierId1").val(data.data.SupplierId);

            $("#txtBarCode1").val(data.data.barCode);


            $('#ModalCreateSingleStock').modal({ backdrop: 'static', keyboard: false })

            $("#ModalCreateSingleStock").modal('show');
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

