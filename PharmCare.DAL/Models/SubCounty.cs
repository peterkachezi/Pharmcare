
namespace PharmCare.DAL.Models
{
    public  partial class SubCounty
    {
        public Guid Id { get; set; }
        public Guid CountyId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
