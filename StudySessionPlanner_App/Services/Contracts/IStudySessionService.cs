using StudySessionPlanner_App.Models;

namespace StudySessionPlanner_App.Services.Contracts
{
    public interface IStudySessionService
    {
        Task<ICollection<StudySession>> GetFilteredSessionsAsync(string? searchTerm, int? topicId);
        Task<ICollection<Topic>> GetAllTopicsAsync();
    }
}