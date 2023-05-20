namespace PharmCare.DTO.InvoiceModule
{
    public class InvoicePaymentDTO
    {
        public Guid Id { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal PaymentMode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
