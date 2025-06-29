using System.ComponentModel.DataAnnotations;

namespace Student.RESTAPI.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credits { get; set; }

        // Navigation property for the join table
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
