

namespace PharmCare.DTO.PrescriptionModule
{
    public class PrescriptionDetailDTO
    {
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string MedicineStrength { get; set; }
        public Guid PatientId { get; set; }
        public string VisitCode { get; set; }
        public string CaseHistory { get; set; }
        public string Medication { get; set; }
        public string Frequency { get; set; }
        public string WhenToTake { get; set; }
        public string NoOfDays { get; set; }
        public byte MedicineDispatchStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string BillNo { get; set; }
        public byte PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }
        public Guid PrescriptionId { get; set; }

        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Total { get; set; }

    }
}
