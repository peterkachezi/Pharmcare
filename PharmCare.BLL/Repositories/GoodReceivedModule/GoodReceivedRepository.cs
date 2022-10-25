

using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.DbContext;
using PharmCare.DTO.StockModule;

namespace PharmCare.BLL.Repositories.GoodReceivedModule
{
    public class GoodReceivedRepository : IGoodReceivedRepository
    {
        private readonly ApplicationDbContext context;

        public GoodReceivedRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<GoodsReceivedHistoryDTO>> GetAll()
        {

            var getData= (from gr in context.GoodsReceivedHistories
                          
                          join med in context.Medicines on gr.MedicineId equals med.Id

                          select new GoodsReceivedHistoryDTO
                          {
                              Id = gr.Id,

                              GRNo = gr.GRNo,

                              CreateDate = gr.CreateDate,

                              CreatedBy = gr.CreatedBy,

                              InvoiceNo = gr.InvoiceNo,

                              SupplierId = gr.SupplierId,

                              MedicineId = gr.MedicineId,

                              Quantity = gr.Quantity,

                              BatchNo = gr.BatchNo,

                              ExpiryDate = gr.ExpiryDate,

                              DateOfManufacture = gr.DateOfManufacture,

                              Total = gr.Total.Value,

                              SellingPrice = gr.SellingPrice.Value, 
                              
                              MedicineName=med.Name,

                          }).ToListAsync(); 

            return await getData;

        }
    }
}
