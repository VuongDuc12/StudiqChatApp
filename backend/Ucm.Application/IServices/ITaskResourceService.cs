using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Application.Dtos;

namespace Ucm.Application.IServices
{
    public interface ITaskResourceService
    {
        Task<IEnumerable<TaskResourceDto>> GetAllAsync();
        Task<TaskResourceDto> GetByIdAsync(int id);
        Task<TaskResourceDto> AddAsync(TaskResourceDto dto);
        Task UpdateAsync(TaskResourceDto dto);
        Task DeleteAsync(int id);
    }
}