

namespace PharmCare.DTO.Accounts.ManufacturerPaymentModule
{
    public  class ManufacturerPaymentDTO
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public string VoucherNo { get; set; }
        public Guid PaymentTypeId { get; set; }
        public Guid? BankId { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
