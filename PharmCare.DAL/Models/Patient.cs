using System.ComponentModel.DataAnnotations;

namespace PharmCare.DAL.Models
{
    public partial class Patient
    {
        [Key]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string PatientNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? Residence { get; set; }
        public string? Gender { get; set; }
        public byte? Status { get; set; }
    

    }
}
