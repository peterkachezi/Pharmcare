using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    public partial class Patient
    {
        [Key]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public string PatientNumber { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? Residence { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? Gender { get; set; }
        [Column("NHIFNo")]
        public string? Nhifno { get; set; }
        [Column("IDNumber")]
        public string? Idnumber { get; set; }
        public Guid? CountyId { get; set; }
        public Guid? SubCountyId { get; set; }

        [ForeignKey(nameof(CountyId))]
        [InverseProperty("Patients")]
        public virtual County? County { get; set; }
        [ForeignKey(nameof(SubCountyId))]
        [InverseProperty("Patients")]
        public virtual SubCounty? SubCounty { get; set; }
    }
}
