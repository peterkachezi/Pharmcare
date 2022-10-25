using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.DTO.LeafSettingModule
{
    public class LeafSettingDTO
    {
        public Guid Id { get; set; }
        public string LeafType { get; set; }
        public string TotalNumberPerBox { get; set; }
        public byte Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
