

namespace PharmCare.DTO.MedicineTypeModule
{
   public class MedicineTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }
    }
}
