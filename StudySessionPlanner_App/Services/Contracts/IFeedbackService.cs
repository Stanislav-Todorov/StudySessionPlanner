using StudySessionPlanner_App.Models;

namespace StudySessionPlanner_App.Services.Contracts
{
    public interface IFeedbackService
    {
        Task<bool> AddFeedbackAsync(int studySessionId, string userId, string comment, int rating);
        Task<bool> HasUserLeftFeedbackAsync(int studySessionId, string userId);
        Task<ICollection<Feedback>> GetFeedbackForSessionAsync(int studySessionId);
    }
}