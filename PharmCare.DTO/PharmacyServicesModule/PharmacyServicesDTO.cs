using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.DTO.PharmacyServicesModule
{
    public class PharmacyServicesDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Charge { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }
        public string NewCharge { get { return Charge.ToString(); } }
    }
}
