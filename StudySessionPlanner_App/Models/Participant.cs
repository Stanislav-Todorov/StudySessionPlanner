using System.ComponentModel.DataAnnotations;
namespace StudySessionPlanner_App.Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public int StudySessionId { get; set; }

        public StudySession StudySession { get; set; } = null!;
    }
}
