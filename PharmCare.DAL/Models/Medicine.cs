using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.DAL.Models
{
    public partial class Medicine
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid MedicalConditionId { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ManufacturerPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SellingPrice { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } 
        public string? UpdatedBy { get; set; }
        public Guid ShelfId { get; set; } 
        public Guid CategoryId { get; set; }
        public Guid UnitId { get; set; }
    }
}
