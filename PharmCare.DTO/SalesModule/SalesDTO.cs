

namespace PharmCare.DTO.SalesModule
{
    public class SalesDTO
    {
        public Guid Id { get; set; }
        public string ReceiptNo { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<SalesDetailsDTO> ListOfSalesDetails { get; set; }
    }
}
