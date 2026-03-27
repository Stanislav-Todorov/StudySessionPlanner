using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Data;
using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services.Contracts;

namespace StudySessionPlanner_App.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFeedbackAsync(int studySessionId, string userId, string comment, int rating)
        {
            bool sessionExists = await _context.StudySessions
                .AnyAsync(s => s.Id == studySessionId);

            if (!sessionExists)
            {
                return false;
            }

            bool alreadyLeftFeedback = await _context.Feedbacks
                .AnyAsync(f => f.StudySessionId == studySessionId && f.UserId == userId);

            if (alreadyLeftFeedback)
            {
                return false;
            }

            Feedback feedback = new Feedback
            {
                StudySessionId = studySessionId,
                UserId = userId,
                Comment = comment,
                Rating = rating
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasUserLeftFeedbackAsync(int studySessionId, string userId)
        {
            return await _context.Feedbacks
                .AnyAsync(f => f.StudySessionId == studySessionId && f.UserId == userId);
        }

        public async Task<ICollection<Feedback>> GetFeedbackForSessionAsync(int studySessionId)
        {
            return await _context.Feedbacks
                .Where(f => f.StudySessionId == studySessionId)
                .Include(f => f.User)
                .ToListAsync();
        }
    }
}