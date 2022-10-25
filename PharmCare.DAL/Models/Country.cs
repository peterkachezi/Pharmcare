using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.DAL.Models
{
    public partial class Country
    {
        public Country()
        {
            Suppliers = new HashSet<Supplier>();
        }

        [Key]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty(nameof(Supplier.Country))]
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
