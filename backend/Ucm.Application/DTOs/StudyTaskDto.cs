using System;
using System.Collections.Generic;
using Ucm.Application.DTOs;

namespace Ucm.Application.Dtos
{
    public class StudyTaskDto
    {
        public int Id { get; set; }
        public int PlanCourseId { get; set; }
        public int StudyPlanId { get; set; }
        public string StudyPlanName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? CourseTopicId { get; set; }
        public string? CourseTopicName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public string Status { get; set; }
        public DateTime? CompletionDate { get; set; }
        public List<StudyLogDto> Logs { get; set; }
        public List<TaskResourceDto> Resources { get; set; }
    }

    // DTO cho việc tạo task mới
    public class CreateStudyTaskRequest
    {
        public int PlanCourseId { get; set; }
        public int? CourseTopicId { get; set; } // Có thể null hoặc 0
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public string Status { get; set; } = "ToDo"; // Default status
    }

    // DTO cho việc cập nhật task
    public class UpdateStudyTaskRequest
    {
        public int? CourseTopicId { get; set; } // Có thể null hoặc 0
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public string Status { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}