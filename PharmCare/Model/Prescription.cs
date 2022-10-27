using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Prescription
    {
        public Prescription()
        {
            PrescriptionDetails = new HashSet<PrescriptionDetail>();
        }

        [Key]
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTime CreateDate { get; set; }
        public byte MedicineDispatchStatus { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? TreatmentFor { get; set; }
        public string? Note { get; set; }
        public string BillNo { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public byte PaymentStatus { get; set; }

        [InverseProperty(nameof(PrescriptionDetail.Prescription))]
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; }
    }
}
