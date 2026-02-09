using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Range(15, 360)]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Required]
        [Display(Name = "Topic")]
        public int TopicId { get; set; }

        [ValidateNever]
        public Topic Topic { get; set; } = null!;

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
