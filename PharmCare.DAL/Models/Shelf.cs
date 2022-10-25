
namespace PharmCare.DAL.Models
{
    public partial class Shelf
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public ICollection<Medicine> Medicines { get; set; }
    }
}
