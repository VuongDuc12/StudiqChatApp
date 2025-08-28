using HotelApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;
using Ucm.Domain.IRepositories;
using Ucm.Infrastructure.Common.Mappers;
using Ucm.Infrastructure.Data;
using Ucm.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ucm.Infrastructure.Repositories
{
    public class CourseRepository : RepositoryBase<Course, CourseEf>, ICourseRepository
    {
        private readonly AppDbContext _context;
        private readonly IEntityEfMapper<Course, CourseEf> _mapper;

        public CourseRepository(AppDbContext context, IEntityEfMapper<Course, CourseEf> mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Course>> GetByIdsAsync(List<int> ids)
        {
            var efItems = await _context.Courses
                                 .Where(c => ids.Contains(c.Id))
                                 .ToListAsync();
            return efItems.Select(_mapper.ToEntity);
        }

        protected override IQueryable<CourseEf> ApplySearchFilter(IQueryable<CourseEf> query, string searchTerm)
        {
            return query.Where(c => c.CourseName.Contains(searchTerm));
        }

        public async Task<Course> AddAsync(Course course)
        {
            var ef = _mapper.ToEf(course);
            _context.Courses.Add(ef);
            await _context.SaveChangesAsync();
            return _mapper.ToEntity(ef);
        }

        public async Task UpdateAsync(Course course)
        {
            var ef = await _context.Courses.FindAsync(course.Id);
            if (ef == null) return;
            // Update properties as needed
            ef.CourseName = course.CourseName;
       
            ef.Credits = course.Credits;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ef = await _context.Courses.FindAsync(id);
            if (ef == null) return;
            _context.Courses.Remove(ef);
            await _context.SaveChangesAsync();
        }
    }
}
