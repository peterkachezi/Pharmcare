using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    [Index(nameof(ShelfId), Name = "IX_Medicines_ShelfId")]
    public partial class Medicine
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
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
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public Guid ShelfId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UnitId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Medicines")]
        public virtual Category Category { get; set; } = null!;
        [ForeignKey(nameof(MedicalConditionId))]
        [InverseProperty("Medicines")]
        public virtual MedicalCondition MedicalCondition { get; set; } = null!;
        [ForeignKey(nameof(ShelfId))]
        [InverseProperty("Medicines")]
        public virtual Shelf Shelf { get; set; } = null!;
        [ForeignKey(nameof(UnitId))]
        [InverseProperty("Medicines")]
        public virtual Unit Unit { get; set; } = null!;
    }
}
