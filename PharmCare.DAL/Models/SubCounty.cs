
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmCare.DAL.Models
{
    public  partial class SubCounty
    {
        public SubCounty()
        {
            Patients = new HashSet<Patient>();
        }

        [Key]
        public Guid Id { get; set; }
        public Guid CountyId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(CountyId))]
        [InverseProperty("SubCounties")]
        public virtual County County { get; set; } = null!;
        [InverseProperty(nameof(Patient.SubCounty))]
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
