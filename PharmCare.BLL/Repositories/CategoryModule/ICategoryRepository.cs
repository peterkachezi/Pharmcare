
using PharmCare.DTO.CategoryModule;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmCare.BLL.Repositories.CategoryModule
{
    public interface ICategoryRepository
    {
        Task<CategoryDTO> Create(CategoryDTO categoryDTO);
        Task<CategoryDTO> Update(CategoryDTO categoryDTO);
        Task<bool> Delete(Guid Id);
        Task<List<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetById(Guid Id);
        Task<bool> CheckIfRecordExist(CategoryDTO categoryDTO);

    }
}