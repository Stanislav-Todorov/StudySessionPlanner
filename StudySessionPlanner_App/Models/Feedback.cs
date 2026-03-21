using System.ComponentModel.DataAnnotations;

namespace StudySessionPlanner_App.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int StudySessionId { get; set; }

        public StudySession StudySession { get; set; } = null!;
    }
}