using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Shared.Common.Pagination;

public class CourseService
{
    private readonly ICourseRepository _repo;

    public CourseService(ICourseRepository repo)
    {
        _repo = repo;
    }

    public async Task<PagedResult<Course>> GetPagedAsync(PaginationParams pagination)
        => await _repo.GetPagedAsync(pagination);

    public async Task<Course> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task AddAsync(Course course)
    {
        await _repo.AddAsync(course);
        await _repo.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _repo.Update(course);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) throw new Exception("Course not found");

        _repo.Delete(existing);
        await _repo.SaveChangesAsync();
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
        => await _repo.GetAllAsync();
}
