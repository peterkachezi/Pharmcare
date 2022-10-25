using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.DTO.ReportModule
{
    public class ReportDTO
    {
        public byte Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CreatedBy { get; set; }
        public Guid CategoryId { get; set; }
    }
}
