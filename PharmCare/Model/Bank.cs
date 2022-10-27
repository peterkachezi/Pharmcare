using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Bank
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string AccountNo { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
