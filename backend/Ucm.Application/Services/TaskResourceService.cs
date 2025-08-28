using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Application.Dtos;
using Ucm.Application.IServices;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;

namespace Ucm.Application.Services
{
    public class TaskResourceService : ITaskResourceService
    {
        private readonly ITaskResourceRepository _repository;

        public TaskResourceService(ITaskResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskResourceDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<TaskResourceDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<TaskResourceDto> AddAsync(TaskResourceDto dto)
        {
            var entity = MapToEntity(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            var saved = await _repository.GetByIdAsync(entity.Id);
            return MapToDto(saved);
        }

        public async Task UpdateAsync(TaskResourceDto dto)
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
        private TaskResourceDto MapToDto(TaskResource entity) =>
            new TaskResourceDto
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                ResourceType = entity.ResourceType,
                ResourceURL = entity.ResourceURL
            };

        private TaskResource MapToEntity(TaskResourceDto dto) =>
            new TaskResource
            {
                Id = dto.Id,
                TaskId = dto.TaskId,
                ResourceType = dto.ResourceType,
                ResourceURL = dto.ResourceURL
            };
    }
}