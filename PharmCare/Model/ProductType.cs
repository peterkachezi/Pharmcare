using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class ProductType
    {
        public ProductType()
        {
            Suppliers = new HashSet<Supplier>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty(nameof(Supplier.ProductType))]
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
