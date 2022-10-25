using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PharmCare.DAL.Models
{
    public partial class PrescriptionDetail
    {
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public Guid PatientId { get; set; }
        public string Frequency { get; set; }
        public string WhenToTake { get; set; }
        public string NoOfDays { get; set; }
        public DateTime CreateDate { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public byte MedicineDispatchStatus { get; set; }

        [ForeignKey("Prescription")]
        public Guid PrescriptionId { get; set; }
        public string BillNo { get; set; }
        public byte PaymentStatus { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
