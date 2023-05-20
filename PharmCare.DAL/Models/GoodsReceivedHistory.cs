using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.DAL.Models
{
    public partial class GoodsReceivedHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public Guid SupplierId { get; set; }
        public Guid GoodsReceivedNoteId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        [Column("GRNo")]
        public string? GRNo { get; set; }
        public string? InvoiceNo { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SellingPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CostPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Total { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalAmount { get; set; }  
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalCostPrice { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public string? BatchNo { get; set; }
        public byte? Status { get; set; }
    }
}
