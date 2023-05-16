

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

/*    $("#divLoader").show();*/

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

    var orderNumber = $('#txtOrderNumber').val();

    console.log(orderNumber);

    var url = "/CyberSection/POS/GetReceipt?OrderNumber=" + orderNumber;

    window.open(window.location.href = url, '_blank');

})

$("#btnCancel").click(function () {

    setTimeout(function () { location.reload() });

})

function hidshowbtncheckout() {

    if (($.trim($("tbody").html()) == ""))

        $("#btnCheckout").hide();

    else
        $("#btnCheckout").show();

}

function SaveTransaction() {

    $("#divLoader").show();

    if ($('#txtTotalAmount').text() < 1) {
        $('#txtTotalAmount').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "There are no items on the list",
            showConfirmButton: true,
        });

        $("#divLoader").hide();

        return false;
    }

    if ($('#txtInvoiceNo').val() == '') {
        $('#txtInvoiceNo').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Invoice No ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }

    if ($('#txtStockInDate').val() == '') {
        $('#txtStockInDate').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter stock In Date ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }

    if ($('#txtDetails').val() == '') {
        $('#txtDetails').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter remarks ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }


    var GoodsReceivedNoteDTO = {};

    ListOfStockDetails = new Array();

    GoodsReceivedNoteDTO.SupplierId = $("#txtSupplierId").val();

    GoodsReceivedNoteDTO.stockInDate = $("#txtStockInDate").val();

    GoodsReceivedNoteDTO.invoiceNo = $("#txtInvoiceNo").val();

    GoodsReceivedNoteDTO.details = $("#txtDetails").val();

    GoodsReceivedNoteDTO.totalAmount = $("#txtTotalAmount").text();


    $("#tblProducts").find("tr:gt(0)").each(function () {

        var GoodsReceivedHistoryDTO = {};

        GoodsReceivedHistoryDTO.MedicineId = $(this).find("td:eq(0)").text();

        GoodsReceivedHistoryDTO.SellingPrice = parseFloat($(this).find("td:eq(2)").text());

        GoodsReceivedHistoryDTO.Quantity = parseFloat($(this).find("td:eq(3)").text());

        GoodsReceivedHistoryDTO.ExpiryDate = $(this).find("td:eq(4)").text();

        GoodsReceivedHistoryDTO.DateOfManufacture = $(this).find("td:eq(5)").text();

        GoodsReceivedHistoryDTO.BatchNo = parseFloat($(this).find("td:eq(6)").text());

        GoodsReceivedHistoryDTO.Total = parseFloat($(this).find("td:eq(7)").text());               

        ListOfStockDetails.push(GoodsReceivedHistoryDTO);

    });

    GoodsReceivedNoteDTO.ListOfStockDetails = ListOfStockDetails;

    console.log(GoodsReceivedNoteDTO);

    $.ajax({

        type: "POST",
        url: "/Admin/StockManager/SaveTransaction/",
        data: GoodsReceivedNoteDTO,

        success: function (response) {

            if (response.success) {

                var grnNumber = response.responseText;

                $("#txtGRNNumber").text(grnNumber);

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



    if ($('#txtExpiryDate').val() < 1) {
        $('#txtExpiryDate').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please select expiry date",
            showConfirmButton: true,
        });

        return false;
    }

    if ($('#txtBatchNo').val() < 1) {
        $('#txtBatchNo').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please select enter Batch No",
            showConfirmButton: true,
        });

        return false;
    }

    var tblItemList = $("#tblProducts");

    var sellingPrice = parseFloat($("[id*=txtSellingPrice]").val());

    var quantity = parseFloat($("[id*=txtQuantity]").val());

    var medicineId = $("[id*=txtMedicineId]").val()

    var itemName = $("#txtMedicineName").val();

    var expiryDate = $("#txtExpiryDate").val();

    var dateOfManufacture = $("#txtDateOfManufacture").val();

    var batchNo = $("#txtBatchNo").val();

    var total = parseFloat(sellingPrice * quantity);

    console.log(medicineId)

    var ItemList =

        "<tr><td hidden>" + medicineId +
       
        "</td><td>" + itemName +
       
        "</td><td>" + parseFloat(sellingPrice).toFixed(2) +
      
        "</td><td>" + parseFloat(quantity).toFixed(2) +

        "</td><td>" + expiryDate +

        "</td><td>" + dateOfManufacture +

        "</td><td>" + batchNo +
        
        "</td><td>" + parseFloat(total).toFixed(2) +       

        "</td><td><a href='#' class='btn-danger  btn-sm' name='Remove' onclick=RemoveItem(this) >Remove </a>  </td></tr>";


    tblItemList.append(ItemList);
    FinaItemTotal();
    ResetItem();
    hidshowbtncheckout();

}

function RemoveItem(Id) {
    $(Id).closest('tr').remove();
    CalculateSubTotal();
    FinaItemTotal();
    hidshowbtncheckout();
}

function ResetItem() {

    $("#txtQuantity").val('0.00');

    $("#txtCurrentQuantity").val('0.00');

    $("#txtExpiryDate").val('');

    $("#txtBatchNo").val('');

    $("#txtTotal").val('0.00');

    $("#txtSellingPrice").val('0.00');
}

function FinaItemTotal() {

    $("#txtTotalAmount").text("0.00");
    var TotalAmount = 0.00;
    $("#tblProducts").find("tr:gt(0)").each(function () {

        var Total = parseFloat($(this).find("td:eq(4)").text());

        TotalAmount += Total;
    });
    $("#txtTotalAmount").text(parseFloat(TotalAmount).toFixed(2));
    $("#txtPaymentTotal").val(parseFloat(TotalAmount).toFixed(2));

}

function CalculateBalance() {

    var FinalAmount = $("#txtTotalAmount").text();

    var PaymentAmount = $("#txtPaymentAmount").val();

    var BalanceAmount = parseFloat(PaymentAmount) - parseFloat(FinalAmount);

    $("#txtBalance").val(parseFloat(BalanceAmount).toFixed(2));

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

            $("#txtstockId").val(data.data.stockId);

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

$("#btnCreateSingleStock").click(function () {

    if ($('#txtInvoiceNo').val() == '') {
        $('#txtInvoiceNo').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter Invoice No ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }

    if ($('#txtStockInDate').val() == '') {
        $('#txtStockInDate').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter stock In Date ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }

    if ($('#txtExpiryDate').val() == '') {
        $('#txtExpiryDate').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter stock expiry Date ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }

    if ($('#txtDetails').val() == '') {
        $('#txtDetails').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter remarks ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }


    if ($('#txtNewStock').val() == '') {
        $('#txtNewStock').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter quantity ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }


    $("#ModalCreateSingleStock").modal('hide');

    $("#divLoader").show();

    var formData = new FormData($('#frmCreateSingleStock').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/StockManager/CreateStock", // NB: Use the correct action name
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

$("#btnUpdateStock").click(function () {



    if ($('#txtNewStock').val() == '') {
        $('#txtNewStock').focus();
        swal({
            position: 'top-end',
            type: "error",
            title: "Please enter quantity ",
            showConfirmButton: true,
        });
        $("#divLoader").hide();

        return false;
    }


    $("#ModalCreateSingleStock").modal('hide');

    $("#divLoader").show();

    var formData = new FormData($('#frmCreateSingleStock').get(0));

    $.ajax({
        type: "POST",
        url: "/Admin/StockManager/UpdateStock", // NB: Use the correct action name
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

function DeleteStockEntry(e) {

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

                url: "/Admin/StockManager/DeleteFromStock/" + id,

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