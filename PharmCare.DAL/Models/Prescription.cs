using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PharmCare.DAL.Models
{
    public partial class Prescription
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTime CreateDate { get; set; }
        public byte MedicineDispatchStatus { get; set; }
        public string CreatedBy { get; set; }
        public string? TreatmentFor { get; set; }
        public string? Note { get; set; }
        public string BillNo { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public byte PaymentStatus { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetail { get; set; }

    }
}
