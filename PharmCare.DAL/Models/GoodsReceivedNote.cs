using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.DAL.Models
{
    public partial class GoodsReceivedNote
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StockInDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public string GRNo { get; set; } = null!;
        public string? InvoiceNo { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public string? Details { get; set; }
    }
}
