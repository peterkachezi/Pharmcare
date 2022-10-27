using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class County
    {
        public County()
        {
            Patients = new HashSet<Patient>();
            SubCounties = new HashSet<SubCounty>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty(nameof(Patient.County))]
        public virtual ICollection<Patient> Patients { get; set; }
        [InverseProperty(nameof(SubCounty.County))]
        public virtual ICollection<SubCounty> SubCounties { get; set; }
    }
}
