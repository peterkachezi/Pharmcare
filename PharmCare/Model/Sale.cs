using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Sale
    {
        [Key]
        public Guid Id { get; set; }
        public string ReceiptNo { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalAmount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AmountPaid { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Balance { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
