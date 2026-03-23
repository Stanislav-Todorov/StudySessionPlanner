using System.ComponentModel.DataAnnotations;

namespace StudySessionPlanner_App.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Url]
        [StringLength(500)]
        public string Url { get; set; } = null!;

        [Required]
        public int StudySessionId { get; set; }

        public StudySession StudySession { get; set; } = null!;
    }
}