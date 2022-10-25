using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Utils;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.SalesModule;

namespace PharmCare.BLL.Repositories.SalesModule
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public SalesRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public SalesDTO SaveSales(SalesDTO salesDTO)
        {
            try
            {
                string receiptNo = TransactionNumber.GetNumber();

                salesDTO.ReceiptNo = receiptNo;

                salesDTO.Id = Guid.NewGuid();

                salesDTO.CreateDate = DateTime.Now;

                var createSale = mapper.Map<Sale>(salesDTO);

                context.Sales.Add(createSale);

                context.SaveChanges();

                var createTransaction = SaveSalesTransactionDetails(salesDTO);

                var romoveStock = RomoveFromStock(salesDTO.ListOfSalesDetails);

                return salesDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public bool RomoveFromStock(IEnumerable<SalesDetailsDTO> listOfSalesDetails)
        {

            try
            {
                SalesDetailsDTO salesDetailsDTO = new SalesDetailsDTO();

                foreach (var item in listOfSalesDetails)
                {
                    var getStock = context.Stocks.FirstOrDefault(x => x.MedicineId == item.MedicineId);

                    if (getStock != null)
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            var currentQuantity = getStock.Quantity;

                            getStock.Quantity = currentQuantity - item.Quantity;

                            transaction.Commit();
                        }

                        context.SaveChanges();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public bool SaveSalesTransactionDetails(SalesDTO salesDTO)
        {
            try
            {
                var listOfItems = new List<SalesDetailsDTO>();

                foreach (var item in salesDTO.ListOfSalesDetails)
                {
                    var data = new SalesDetailsDTO
                    {
                        Id = Guid.NewGuid(),

                        CreateDate = DateTime.Now,

                        ReceiptNo = salesDTO.ReceiptNo,

                        SaleId = salesDTO.Id,

                        Total = (item.Quantity) * (item.SellingPrice),

                        MedicineId = item.MedicineId,

                        Quantity = item.Quantity,

                        CreatedBy = salesDTO.CreatedBy,

                        SellingPrice = item.SellingPrice,
                    };

                    listOfItems.Add(data);
                }

                var sales = mapper.Map<List<SalesDetail>>(listOfItems);

                context.SalesDetails.AddRange(sales);

                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<List<SalesDetailsDTO>> GetSalesDetailsByReceiptNo(string ReceiptNo)
        {
            try
            {
                var data = (from sale in context.SalesDetails

                            join medic in context.Medicines on sale.MedicineId equals medic.Id

                            join unit in context.Units on medic.UnitId equals unit.Id    

                            where sale.ReceiptNo == ReceiptNo

                            select new SalesDetailsDTO
                            {
                                Id = sale.Id,

                                Quantity = sale.Quantity,

                                SellingPrice = sale.SellingPrice,

                                Total = sale.Total,

                                CreateDate = sale.CreateDate,

                                ReceiptNo = sale.ReceiptNo,

                                MedicineName = medic.Name +" " + unit.Name + " " + unit.UnitValue,

                            }).ToListAsync();

                return await data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<SalesDTO> GetSaleByReceiptNo(string ReceiptNo)
        {
            try
            {
                var getSale = (from sale in context.Sales

                               join user in context.AppUsers on sale.CreatedBy equals user.Id

                               where sale.ReceiptNo == ReceiptNo

                               select new SalesDTO
                               {
                                   Id = sale.Id,

                                   ReceiptNo = sale.ReceiptNo,

                                   TotalAmount = sale.TotalAmount.Value,

                                   AmountPaid = sale.AmountPaid.Value,

                                   Balance = sale.Balance.Value,

                                   CreateDate = sale.CreateDate,

                                   CreatedByName = user.FirstName + " " + user.LastName

                               }).FirstOrDefaultAsync();

                return await getSale;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<List<SalesDetailsDTO>> GetAllSalesDetails()
        {
            try
            {
                var data = (from sale in context.SalesDetails

                            join medic in context.Medicines on sale.MedicineId equals medic.Id

                            join unit in context.Units on medic.UnitId equals unit.Id

                            select new SalesDetailsDTO
                            {
                                Id = sale.Id,

                                Quantity = sale.Quantity,

                                SellingPrice = sale.SellingPrice,

                                Total = sale.Total,

                                CreateDate = sale.CreateDate,

                                ReceiptNo = sale.ReceiptNo,

                                MedicineName = medic.Name,

                                UnitName=unit.Name + " " + unit.UnitValue,
                                

                            }).ToListAsync();

                return await data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
