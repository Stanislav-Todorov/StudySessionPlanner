using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Models;

namespace StudySessionPlanner_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudySession> StudySessions { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
    }
}