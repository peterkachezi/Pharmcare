

namespace PharmCare.DTO.PrescriptionModule
{
    public class IssueMedicineDTO
    {
        public Guid PrescriptionId { get; set; }
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
