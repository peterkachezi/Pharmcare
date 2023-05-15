using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Utils;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.ReportModule;
using PharmCare.DTO.StockModule;
using System.Security.Cryptography;

namespace PharmCare.BLL.Repositories.StockModule
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public StockRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<GoodsReceivedNoteDTO> SaveStock(GoodsReceivedNoteDTO goodsReceivedNoteDTO)
        {
            try
            {
                string grnNo = TransactionNumber.GetNumber();

                goodsReceivedNoteDTO.GRNo = grnNo;

                goodsReceivedNoteDTO.Id = Guid.NewGuid();

                goodsReceivedNoteDTO.CreateDate = DateTime.Now;

                var patient = mapper.Map<GoodsReceivedNote>(goodsReceivedNoteDTO);

                context.GoodsReceivedNotes.Add(patient);

                await context.SaveChangesAsync();

                var listOfItems = new List<GoodsReceivedHistoryDTO>();

                foreach (var item in goodsReceivedNoteDTO.ListOfStockDetails)
                {
                    var data = new GoodsReceivedHistoryDTO
                    {
                        Id=Guid.NewGuid(),

                        GRNo = goodsReceivedNoteDTO.GRNo,

                        CreateDate = DateTime.Now,

                        CreatedBy = goodsReceivedNoteDTO.CreatedBy,

                        InvoiceNo = goodsReceivedNoteDTO.InvoiceNo,

                        TotalAmount = goodsReceivedNoteDTO.TotalAmount,

                        SupplierId = goodsReceivedNoteDTO.SupplierId,

                        MedicineId = item.MedicineId,

                        Quantity = item.Quantity,

                        BatchNo = item.BatchNo,

                        ExpiryDate = item.ExpiryDate,

                        DateOfManufacture = item.DateOfManufacture,

                        Total = (item.Quantity) * (item.SellingPrice),

                        SellingPrice = item.SellingPrice,

                        GoodsReceivedNoteId = goodsReceivedNoteDTO.Id,
                    };

                    listOfItems.Add(data);
                }

                CreateStock(goodsReceivedNoteDTO);

                BulkGoodsReceivedHistories(listOfItems);

                return goodsReceivedNoteDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        private void BulkGoodsReceivedHistories(List<GoodsReceivedHistoryDTO> listOfItems)
        {
            try
            {



                var data = mapper.Map<List<GoodsReceivedHistory>>(listOfItems);

                context.GoodsReceivedHistories.AddRange(data);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public void CreateStock(GoodsReceivedNoteDTO goodsReceivedNoteDTO)
        {
            try
            {
                foreach (var item in goodsReceivedNoteDTO.ListOfStockDetails)
                {
                    var isMedicineInStock = context.Stocks.FirstOrDefault(x => x.MedicineId == item.MedicineId);

                    if (isMedicineInStock == null)
                    {

                        item.Id = Guid.NewGuid();

                        item.CreateDate = DateTime.Now;

                        item.CreatedBy = goodsReceivedNoteDTO.CreatedBy;

                        var create = mapper.Map<Stock>(item);

                        context.Stocks.Add(create);

                        context.SaveChanges();
                    }

                    if (isMedicineInStock != null)
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {

                            isMedicineInStock.Quantity = (item.Quantity) + (isMedicineInStock.Quantity);

                            isMedicineInStock.UpdatedBy = goodsReceivedNoteDTO.CreatedBy;

                            isMedicineInStock.UpdatedDate = DateTime.Now;

                            transaction.Commit();

                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public async Task<GoodsReceivedHistoryDTO> CreateSingleEntry(GoodsReceivedHistoryDTO stockDetailDTO)
        {
            try
            {
                string grnNo = TransactionNumber.GetNumber();

                stockDetailDTO.GRNo = grnNo;

                stockDetailDTO.Id = Guid.NewGuid();

                stockDetailDTO.CreateDate = DateTime.Now;

                var isMedicineInStock = await context.Stocks.FirstOrDefaultAsync(x => x.MedicineId == stockDetailDTO.MedicineId);

                if (isMedicineInStock == null)
                {

                    var create = mapper.Map<Stock>(stockDetailDTO);

                    context.Stocks.Add(create);

                    await context.SaveChangesAsync();

                    return stockDetailDTO;
                }

                if (isMedicineInStock != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {

                        isMedicineInStock.Quantity = (stockDetailDTO.Quantity) + (isMedicineInStock.Quantity);

                        isMedicineInStock.UpdatedBy = stockDetailDTO.CreatedBy;

                        isMedicineInStock.UpdatedDate = DateTime.Now;

                        transaction.Commit();

                    }
                    await context.SaveChangesAsync();

                    return stockDetailDTO;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;

            }
        }
        public List<GoodsReceivedHistoryDTO> GetExpiredProducts()
        {
            try
            {
                var endDate = DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59);

                var data = context.GoodsReceivedHistories.Where(x => x.ExpiryDate < endDate).ToList();

                var expiries = (from exp in data

                                join med in context.Medicines on exp.MedicineId equals med.Id

                                join unit in context.Units on med.UnitId equals unit.Id

                                join shelf in context.Shelves on med.ShelfId equals shelf.Id

                                join category in context.Categories on med.CategoryId equals category.Id                                                      

                                join sup in context.Suppliers  on exp.SupplierId equals sup.Id

                                select new GoodsReceivedHistoryDTO
                                {
                                    Id = exp.Id,

                                    GRNo = exp.GRNo,

                                    CreateDate = exp.CreateDate,

                                    CreatedBy = exp.CreatedBy,

                                    InvoiceNo = exp.InvoiceNo,

                                    TotalAmount = exp.TotalAmount.Value,

                                    SupplierId = exp.SupplierId,

                                    MedicineId = exp.MedicineId,

                                    Quantity = exp.Quantity,

                                    BatchNo = exp.BatchNo,

                                    ExpiryDate = exp.ExpiryDate,

                                    DateOfManufacture = exp.DateOfManufacture,

                                    Total = exp.Total.Value,

                                    SellingPrice = exp.SellingPrice.Value,

                                    ManufacturerPrice = med.ManufacturerPrice,

                                    GoodsReceivedNoteId = exp.Id,

                                    MedicineName = med.Name + " " + unit.Name + " " + unit.UnitValue,

                                    ShelfName = shelf.Name,

                                    CategoryName = category.Name,

                                    NameOfSupplier = sup.Name,

                                }).ToList();

                return expiries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
    }
}
