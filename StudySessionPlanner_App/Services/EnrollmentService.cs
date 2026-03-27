using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Data;
using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services.Contracts;

namespace StudySessionPlanner_App.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EnrollUserAsync(int studySessionId, string userId)
        {
            bool studySessionExists = await _context.StudySessions
                .AnyAsync(s => s.Id == studySessionId);

            if (!studySessionExists)
            {
                return false;
            }

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudySessionId == studySessionId && e.UserId == userId);

            if (alreadyEnrolled)
            {
                return false;
            }

            Enrollment enrollment = new Enrollment
            {
                StudySessionId = studySessionId,
                UserId = userId,
                JoinedOn = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsUserEnrolledAsync(int studySessionId, string userId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudySessionId == studySessionId && e.UserId == userId);
        }

        public async Task<int> GetEnrollmentCountAsync(int studySessionId)
        {
            return await _context.Enrollments
                .CountAsync(e => e.StudySessionId == studySessionId);
        }
    }
}