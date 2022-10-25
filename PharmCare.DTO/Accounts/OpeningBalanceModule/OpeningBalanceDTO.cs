

namespace PharmCare.DTO.Accounts.OpeningBalanceModule
{
    public  class OpeningBalanceDTO
    {
        public Guid  Id { get; set; }
        public string VoucherNo{ get; set; }
        public string AccountHead{ get; set; }
        public decimal Amount{ get; set; }
        public string Remarks { get; set; }
        public DateTime OpeningDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
