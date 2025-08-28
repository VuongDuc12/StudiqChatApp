using Ucm.Application.DTOs.Course;

namespace Ucm.Application.Dtos
{
    public class StudyPlanCourseDto
    {
        public int Id { get; set; }
        public int StudyPlanId { get; set; }
        public int CourseId { get; set; }
        public Guid? UserId { get; set; }
        public CourseDto Course { get; set; }
        // Add Task DTOs if needed
    }

    // Request dùng cho tạo mới
    public class StudyPlanCourseCreateRequest
    {
        public int StudyPlanId { get; set; }
        public int CourseId { get; set; }
    }
}