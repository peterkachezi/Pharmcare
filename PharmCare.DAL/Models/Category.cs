﻿using System.ComponentModel.DataAnnotations;


namespace PharmCare.DAL.Models
{
    public partial class Category
    {
       
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public byte Status { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }    
        public virtual ICollection<Medicine> Medicines { get; set; }

    }
}
