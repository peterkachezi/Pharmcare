

namespace PharmCare.DTO.StockModule
{
    public class GoodsReceivedNoteDTO
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StockInDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string GRNo { get; set; }
        public string InvoiceNo { get; set; }
        public decimal TotalAmount { get; set; }
        public string Details { get; set; }
        public IEnumerable<GoodsReceivedHistoryDTO> ListOfStockDetails { get; set; }
    }
}
