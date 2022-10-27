using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class SalesDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public Guid SaleId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string ReceiptNo { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SellingPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }
    }
}
