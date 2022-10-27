using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    [Index(nameof(PrescriptionId), Name = "IX_PrescriptionDetails_PrescriptionId")]
    public partial class PrescriptionDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public Guid PatientId { get; set; }
        public string Frequency { get; set; } = null!;
        public string WhenToTake { get; set; } = null!;
        public string NoOfDays { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; } = null!;
        public byte MedicineDispatchStatus { get; set; }
        public Guid PrescriptionId { get; set; }
        public string BillNo { get; set; } = null!;
        public byte PaymentStatus { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        [ForeignKey(nameof(PrescriptionId))]
        [InverseProperty("PrescriptionDetails")]
        public virtual Prescription Prescription { get; set; } = null!;
    }
}
