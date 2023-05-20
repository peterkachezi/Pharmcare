using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.DAL.Models
{
    [Index(nameof(CountryId), Name = "IX_Suppliers_CountryId")]
    [Index(nameof(ProductTypeId), Name = "IX_Suppliers_ProductTypeId")]
    public partial class Supplier
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhysicalAddress { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Town { get; set; }
        public string? SupplierNo { get; set; }
        public byte? Status { get; set; }
        public Guid CountryId { get; set; }
        public Guid ProductTypeId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty("Suppliers")]
        public virtual Country Country { get; set; } = null!;


        [ForeignKey(nameof(ProductTypeId))]
        [InverseProperty("Suppliers")]
        public virtual ProductType ProductType { get; set; } = null!;
    }
}
