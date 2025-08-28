using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Application.Dtos;
using Ucm.Application.DTOs;
using Ucm.Application.IServices;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;

namespace Ucm.Application.Services
{
    public class StudyLogService : IStudyLogService
    {
        private readonly IStudyLogRepository _repository;

        public StudyLogService(IStudyLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<StudyLogDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<StudyLogDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<StudyLogDto> AddAsync(StudyLogDto dto)
        {
            var entity = MapToEntity(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            var saved = await _repository.GetByIdAsync(entity.Id);
            return MapToDto(saved);
        }

        public async Task UpdateAsync(StudyLogDto dto)
        {
            var entity = MapToEntity(dto);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }

        // Mapping helpers
        private StudyLogDto MapToDto(StudyLog entity) =>
            new StudyLogDto
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                ActualTimeSpent = entity.ActualTimeSpent,
                LogDate = entity.LogDate
            };

        private StudyLog MapToEntity(StudyLogDto dto) =>
            new StudyLog
            {
                Id = dto.Id,
                TaskId = dto.TaskId,
                ActualTimeSpent = dto.ActualTimeSpent,
                LogDate = dto.LogDate
            };
    }
}