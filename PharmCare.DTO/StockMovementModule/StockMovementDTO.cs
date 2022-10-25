using System;
using System.Collections.Generic;
using System.Text;

namespace PharmCare.DTO.StockMovementModule
{
    public class StockMovementDTO
    {
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string GRNo { get; set; }
        public byte Status { get; set; }


        public string MedicineName { get; set; }
        public string GenericName { get; set; }
        public Guid UnitId { get; set; }
        public Guid ShelfId { get; set; }
        public string Note { get; set; }
        public Guid CategoryId { get; set; }
        public Guid MedicineTypeId { get; set; }
        public Guid SupplierId { get; set; }
        public decimal ManufacturerPrice { get; set; }
        public string ManufacturerName { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Vat { get; set; }

    }
}
