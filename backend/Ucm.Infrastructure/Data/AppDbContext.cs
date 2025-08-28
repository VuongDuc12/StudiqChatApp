using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ucm.Infrastructure.Data.Models;

namespace Ucm.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUserEF, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CourseEf> Courses { get; set; }
        public DbSet<CourseTopicEf> CourseTopics { get; set; }
        public DbSet<StudyPlanEf> StudyPlans { get; set; }
        public DbSet<StudyPlanCourseEf> StudyPlanCourses { get; set; }
        public DbSet<StudyTaskEf> StudyTasks { get; set; }
        public DbSet<StudyLogEf> StudyLogs { get; set; }
        public DbSet<TaskResourceEf> TaskResources { get; set; }
        public DbSet<NotificationEf> Notifications { get; set; }
        public DbSet<TemplateStudyPlanEf> TemplateStudyPlans { get; set; }
        public DbSet<TemplateStudyTaskEf> TemplateStudyTasks { get; set; }
        public DbSet<TemplateTaskResourceEf> TemplateTaskResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Đổi tên bảng cho Identity (PostgreSQL thường dùng snake_case)
            builder.Entity<AppUserEF>().ToTable("users");
            builder.Entity<IdentityRole<Guid>>().ToTable("roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");

            // Cấu hình các quan hệ, giữ nguyên logic nhưng có thể cần chú ý về tên cột/bảng nếu migration lỗi
            builder.Entity<StudyPlanCourseEf>()
                .HasOne(p => p.StudyPlan)
                .WithMany(sp => sp.PlanCourses)
                .HasForeignKey(p => p.StudyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudyPlanCourseEf>()
                .HasOne(p => p.Course)
                .WithMany()
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudyPlanCourseEf>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudyTaskEf>()
                .HasOne(t => t.PlanCourse)
                .WithMany(pc => pc.Tasks)
                .HasForeignKey(t => t.PlanCourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudyTaskEf>()
                .HasOne(t => t.CourseTopic)
                .WithMany()
                .HasForeignKey(t => t.CourseTopicId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<StudyLogEf>()
                .HasOne(l => l.Task)
                .WithMany(t => t.Logs)
                .HasForeignKey(l => l.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TaskResourceEf>()
                .HasOne(r => r.Task)
                .WithMany(t => t.Resources)
                .HasForeignKey(r => r.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NotificationEf>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NotificationEf>()
                .HasOne(n => n.RelatedTask)
                .WithMany()
                .HasForeignKey(n => n.RelatedTaskId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TemplateStudyPlanEf>()
                .HasOne(t => t.Course)
                .WithMany()
                .HasForeignKey(t => t.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TemplateStudyTaskEf>()
                .HasOne(t => t.TemplatePlan)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.TemplatePlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TemplateTaskResourceEf>()
                .HasOne(r => r.TemplateTask)
                .WithMany(t => t.Resources)
                .HasForeignKey(r => r.TemplateTaskId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}