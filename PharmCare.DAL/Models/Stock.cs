using System.ComponentModel.DataAnnotations;

namespace PharmCare.DAL.Models
{
    public partial class Stock
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }      
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
    }
}
