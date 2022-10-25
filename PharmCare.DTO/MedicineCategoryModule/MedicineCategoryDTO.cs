using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.DTO.MedicineCategoryModule
{
  public  class MedicineCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreateDate { get; set; }
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }

    }
}
