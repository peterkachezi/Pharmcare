using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Bill
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid PrescriptionId { get; set; }
        public string BillNo { get; set; } = null!;
        public byte Status { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
