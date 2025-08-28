using HotelApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;
using System;
using System.Linq.Expressions;
using Ucm.Shared.Common.Pagination;

namespace Ucm.Infrastructure.Repositories
{
    public class StudyPlanCourseRepository : RepositoryBase<StudyPlanCourse, StudyPlanCourseEf>, IStudyPlanCourseRepository
    {
        public StudyPlanCourseRepository(AppDbContext context, IEntityEfMapper<StudyPlanCourse, StudyPlanCourseEf> mapper)
            : base(context, mapper) { }
            
        public async Task<IEnumerable<StudyPlanCourse>> GetByUserIdAsync(Guid userId)
        {
            var efItems = await _context.StudyPlanCourses
                .Include(spc => spc.Tasks)
                .Where(spc => spc.UserId == userId)
                .ToListAsync();
            return efItems.Select(_mapper.ToEntity);
        }

        public async Task<IEnumerable<StudyPlanCourse>> GetByStudyPlanIdAsync(int studyPlanId)
        {
            var efItems = await _context.StudyPlanCourses
                .Include(spc => spc.Tasks)
                .Where(spc => spc.StudyPlanId == studyPlanId)
                .ToListAsync();
            return efItems.Select(_mapper.ToEntity);
        }

        public async Task<int> CountByStudyPlanIdAsync(int studyPlanId)
        {
            return await _context.StudyPlanCourses
                .CountAsync(spc => spc.StudyPlanId == studyPlanId);
        }

        public async Task<StudyPlanCourse> GetByIdWithCourseAsync(int id)
        {
            var ef = await _context.StudyPlanCourses
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);
            return ef == null ? null : _mapper.ToEntity(ef);
        }

        public async Task<IEnumerable<StudyPlanCourse>> GetAllByUserIdAsync(Guid userId)
        {
            Console.WriteLine($"Getting all plan courses for user: {userId}");
            var query = _context.StudyPlanCourses
                .Include(pc => pc.StudyPlan)
                .Include(pc => pc.Course)
                .Where(pc => pc.UserId == userId);

            var entities = await query.ToListAsync();
            Console.WriteLine($"Found {entities.Count} plan courses");

            return entities.Select(_mapper.ToEntity);
        }
    }
}