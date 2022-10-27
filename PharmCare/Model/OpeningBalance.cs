using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class OpeningBalance
    {
        [Key]
        public Guid Id { get; set; }
        public string VoucherNo { get; set; } = null!;
        public string AccountHead { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public string Remarks { get; set; } = null!;
        public DateTime OpeningDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
