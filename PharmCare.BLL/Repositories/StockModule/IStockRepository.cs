﻿using PharmCare.DTO.StockModule;

namespace PharmCare.BLL.Repositories.StockModule
{
    public interface IStockRepository
    {
        Task<GoodsReceivedNoteDTO> SaveStock(GoodsReceivedNoteDTO goodsReceivedNoteDTO);
        Task<GoodsReceivedHistoryDTO> CreateSingleEntry(GoodsReceivedHistoryDTO stockDetailDTO);
        Task<GoodsReceivedHistoryDTO> UpdateStock(GoodsReceivedHistoryDTO stockDetailDTO);
        Task<bool> DeleteFromStock(Guid Id);
        List<GoodsReceivedHistoryDTO> GetExpiredProducts();
    }
}