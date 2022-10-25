using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.DTO.PatientModule
{
    public class PatientDTO
    {
        public System.Guid Id { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string PatientNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FullName => FirstName + "  " + LastName;
        public string CreatedByName { get; set; }
        public string Residence { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Gender { get; set; }
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }

        public int Age
        {
            get
            {
                int age = DateTime.Now.Subtract(DateOfBirth).Days;

                age = (age / 365);

                return age;

            }
        }


    }
}
