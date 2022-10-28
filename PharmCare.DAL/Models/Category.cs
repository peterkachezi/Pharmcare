using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmCare.DAL.Models
{
    public partial class Category
    {
        public Category()
        {
            Medicines = new HashSet<Medicine>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public byte Status { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty(nameof(Medicine.Category))]
        public virtual ICollection<Medicine> Medicines { get; set; }

    }
}
