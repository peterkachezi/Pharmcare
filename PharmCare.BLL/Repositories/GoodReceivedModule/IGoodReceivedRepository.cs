using PharmCare.DTO.StockModule;

namespace PharmCare.BLL.Repositories.GoodReceivedModule
{
    public interface IGoodReceivedRepository
    {
        Task<List<GoodsReceivedHistoryDTO>> GetAll();
    }
}