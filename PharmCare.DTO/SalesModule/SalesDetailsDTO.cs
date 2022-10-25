

using System.Xml.Linq;

namespace PharmCare.DTO.SalesModule
{
    public class SalesDetailsDTO
    {
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public string MedicineName { get; set; }
        public Guid SaleId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string ReceiptNo { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public Guid PrescriptionId { get; set; }
        public string NewCreateDate
        {
            get
            {
                return CreateDate.ToString("dd MMMM yyyy h:mm tt");
            }
        }

        public string UnitName { get; set; }
        public string MedicineFullName => MedicineName + " " + UnitName;

    }
}
