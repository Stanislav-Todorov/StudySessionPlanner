using System.ComponentModel.DataAnnotations;

namespace StudySessionPlanner_App.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int StudySessionId { get; set; }

        public StudySession StudySession { get; set; } = null!;

        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
    }
}