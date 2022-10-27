using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Shelf
    {
        public Shelf()
        {
            Medicines = new HashSet<Medicine>();
        }

        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        [InverseProperty(nameof(Medicine.Shelf))]
        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
