

namespace PharmCare.DTO.SupplierModule
{
   public class SupplierDTO
    {
        public Guid Id { get; set; }
        public Guid ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public Guid ContactId { get; set; }
        public string SupplierNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CountryId { get; set; }
        public Guid ContactCountryId { get; set; }
        public string CountryName { get; set; }
        public string Town { get; set; }
        public string PhysicalAddress { get; set; }
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }
    }
}
