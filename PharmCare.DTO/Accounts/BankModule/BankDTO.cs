

namespace PharmCare.DTO.Accounts.BankModule
{
    public  class BankDTO
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Branch { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
