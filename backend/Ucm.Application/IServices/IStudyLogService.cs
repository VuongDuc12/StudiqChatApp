using System.Collections.Generic;
using System.Threading.Tasks;
using Ucm.Application.Dtos;
using Ucm.Application.DTOs;

namespace Ucm.Application.IServices
{
    public interface IStudyLogService
    {
        Task<IEnumerable<StudyLogDto>> GetAllAsync();
        Task<StudyLogDto> GetByIdAsync(int id);
        Task<StudyLogDto> AddAsync(StudyLogDto dto);
        Task UpdateAsync(StudyLogDto dto);
        Task DeleteAsync(int id);
    }
}