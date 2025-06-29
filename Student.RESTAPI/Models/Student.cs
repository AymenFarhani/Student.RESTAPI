using System.ComponentModel.DataAnnotations;

namespace Student.RESTAPI.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50, ErrorMessage = "FullName cannot exceed 50 characters.")]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }

        // Navigation property for the join table
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
