using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.DTO.SalesOrderModule
{
    public class SalesDetailsDTO
    {
        public System.Guid Id { get; set; }
        public System.Guid MedicineId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }


    }
}
