using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Unit
    {
        public Unit()
        {
            Medicines = new HashSet<Medicine>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int UnitValue { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty(nameof(Medicine.Unit))]
        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
