

namespace PharmCare.DTO.StockModule
{
    public class GoodsReceivedHistoryDTO
    {
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public Guid SupplierId { get; set; }
        public Guid GoodsReceivedNoteId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StockInDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateOfManufacture{ get; set; }
        public string BatchNo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string GRNo { get; set; }
        public string InvoiceNo { get; set; }
        public string MedicineName { get; set; }
        public string GenericName { get; set; }
        public decimal ManufacturerPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Total { get; set; }
        public string CategoryName { get; set; }
        public string ShelfName { get; set; }
        public string NameOfSupplier { get; set; }

    }
}
