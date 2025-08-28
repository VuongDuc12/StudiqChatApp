
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using Ucm.Infrastructure.Repositories;

namespace Ucm.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký các Repository Implementation nếu có
            // services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudyPlanRepository, StudyPlanRepository>();
            services.AddScoped<IStudyTaskRepository, StudyTaskRepository>();
            services.AddScoped<IStudyPlanCourseRepository, StudyPlanCourseRepository>();
            services.AddScoped<ICourseTopicRepository, CourseTopicRepository>();
            services.AddScoped<ITaskResourceRepository, TaskResourceRepository>();
            services.AddScoped<IStudyLogRepository, StudyLogRepository>();


            services.AddScoped<IEntityEfMapper<Course, CourseEf>, CourseEntityEfMapper>();
            services.AddScoped<IEntityEfMapper<CourseTopic, CourseTopicEf>, CourseTopicEntityEfMapper>();
            services.AddScoped<IEntityEfMapper<StudyPlanCourse, StudyPlanCourseEf>, StudyPlanCourseEntityEfMapper>();
            services.AddScoped<IEntityEfMapper<StudyPlan, StudyPlanEf>, StudyPlanEntityEfMapper>();
            services.AddScoped<IEntityEfMapper<StudyTask, StudyTaskEf>, StudyTaskEntityEfMapper>();
            services.AddScoped<IEntityEfMapper<StudyLog, StudyLogEf>, StudyLogEntityEfMapper>();
            services.AddScoped<IEntityEfMapper<TaskResource, TaskResourceEf>, TaskResourceEntityEfMapper>();

            return services;
        }
    }
}
