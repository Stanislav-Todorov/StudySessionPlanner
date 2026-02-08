using StudySessionPlanner_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudySessionPlanner_App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudySession> StudySessions { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;

    }
}
