using PharmCare.DTO.StockModule;

namespace PharmCare.BLL.Repositories.StockModule
{
    public interface IStockRepository
    {
        Task<GoodsReceivedNoteDTO> SaveStock(GoodsReceivedNoteDTO goodsReceivedNoteDTO);
        Task<GoodsReceivedHistoryDTO> CreateSingleEntry(GoodsReceivedHistoryDTO stockDetailDTO);
        List<GoodsReceivedHistoryDTO> GetExpiredProducts();
    }
}