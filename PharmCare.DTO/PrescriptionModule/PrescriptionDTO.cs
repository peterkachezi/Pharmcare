using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.DTO.PrescriptionModule
{
    public class PrescriptionDTO
    {
        public Guid Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid PatientId { get; set; }
        public string VisitCode { get; set; }
        public DateTime CreateDate { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string TreatmentFor { get; set; }
        public string Note { get; set; }
        public byte MedicineDispatchStatus { get; set; }
        public string PrescriptionFor { get; set; }
        public string TreatmentForName { get; set; }
        public Guid TreatmentForId { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string PatientIdNumber { get; set; }
        public string PatientRegCode { get; set; }
        public string BillNo { get; set; }
        public decimal TotalAmount { get; set; }
        public byte PaymentStatus { get; set; }
        public ICollection<PrescriptionDetailDTO> PrescriptionDetailDTO { get; set; }
    }
}
