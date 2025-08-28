using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ucm.Application.IServices;
using Ucm.Application.Services;
namespace Ucm.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Đăng ký các dịch vụ Application Layer ở đây
            // services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IStudyPlanService, StudyPlanService>();
            services.AddScoped<IStudyPlanCourseService, StudyPlanCourseService>();
            services.AddScoped<IStudyTaskService, StudyTaskService>();
            services.AddScoped<IStudyLogService, StudyLogService>();
            services.AddScoped<ITaskResourceService, TaskResourceService>();

            services.AddScoped<CourseService>();
            services.AddScoped<CourseTopicService>();
            return services;
        }
    }
}
