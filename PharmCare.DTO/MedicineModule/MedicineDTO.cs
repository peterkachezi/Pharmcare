

namespace PharmCare.DTO.MedicineModule
{
    public class MedicineDTO
    {
        public Guid Id { get; set; }
        public Guid StockId { get; set; }
        public string Name { get; set; }
        public Guid MedicalConditionId { get; set; } 
        public string MedicalConditionName { get; set; } 
        public Guid ShelfId { get; set; }
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }
        public string ShelfName { get; set; } 
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string MedicineTypeName { get; set; }
        public string ManufacturerName { get; set; }
        public decimal ManufacturerPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StockDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedBy { get; set; }
        public string MedicineFullName => Name + " " + UnitName;
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }
        public int Quantity { get; set; }
        public Guid PrescriptionId { get; set; }
    }
}
