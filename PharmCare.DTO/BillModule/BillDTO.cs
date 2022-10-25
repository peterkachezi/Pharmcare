

namespace PharmCare.DTO.BillModule
{
    public  class BillDTO
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid PrescriptionId { get; set; }
        public string BillNo { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
