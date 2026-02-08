using System.ComponentModel.DataAnnotations;
namespace StudySessionPlanner_App.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; } = null!;

        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
    }
}
