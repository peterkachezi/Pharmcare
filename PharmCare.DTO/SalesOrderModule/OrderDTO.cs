using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.DTO.SalesOrderModule
{
   public class OrderDTO
    {  
        public string CreatedBy { get; set; }
        public string GRNo { get; set; }

        public IEnumerable<SalesDetailsDTO> ListOfSalesDetails { get; set; }
    }
}
