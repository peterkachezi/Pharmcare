using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SellingPrice { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public byte? Status { get; set; }
    }
}
