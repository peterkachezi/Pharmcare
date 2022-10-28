
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmCare.DAL.Models
{
    public  partial class SubCounty
    {
       

        [Key]
        public Guid Id { get; set; }
        public Guid CountyId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual County County { get; set; } = null!;
 
    }
}
