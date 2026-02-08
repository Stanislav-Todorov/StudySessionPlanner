using System.ComponentModel.DataAnnotations;
namespace StudySessionPlanner_App.Models
{
    public class StudySession
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public DateTime StartTime { get; set; }

        [Range(15, 360)]
        public int DurationMinutes { get; set; }

        [Required]
        public int TopicId { get; set; }

        public Topic Topic { get; set; } = null!;

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
