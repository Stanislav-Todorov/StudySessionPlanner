using Microsoft.AspNetCore.Identity;

namespace StudySessionPlanner_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}