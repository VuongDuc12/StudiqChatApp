using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Application.DTOs.Course
{
    // DTO dùng để tạo Course
    public class CreateCourseDto
    {
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }
    }

    // DTO dùng để update Course
    public class UpdateCourseDto
    {
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }
    }

    // DTO để trả về cho client (nếu cần)
    public class CourseDto
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }
    }

}
