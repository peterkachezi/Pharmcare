using PharmCare.DTO.PatientModule;


namespace PharmCare.DTO.PrescriptionModule
{
    public class PrescriptionProfileDTO
    {
        public PatientDTO patientDTO { get; set; }
        public PrescriptionDTO prescription { get; set; }
        public List<PrescriptionDetailDTO> prescriptionDetails { get; set; }
    }
}
