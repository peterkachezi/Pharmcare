using PharmCare.DTO.SalesModule;


namespace PharmCare.BLL.Repositories.SalesModule
{
    public interface ISalesRepository
    {
        SalesDTO SaveSales(SalesDTO salesDTO);  
        Task<List<SalesDetailsDTO>> GetSalesDetailsByReceiptNo(string ReceiptNo);
        Task<List<SalesDetailsDTO>> GetAllSalesDetails();
        Task<SalesDTO> GetSaleByReceiptNo(string ReceiptNo);

    }
}