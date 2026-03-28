using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Data;
using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services.Contracts;

namespace StudySessionPlanner_App.Services
{
    public class StudySessionService : IStudySessionService
    {
        private readonly ApplicationDbContext _context;

        public StudySessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<StudySession>> GetFilteredSessionsAsync(string? searchTerm, int? topicId)
        {
            IQueryable<StudySession> query = _context.StudySessions
                .Include(s => s.Topic);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Title.Contains(searchTerm));
            }

            if (topicId.HasValue)
            {
                query = query.Where(s => s.TopicId == topicId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<ICollection<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}