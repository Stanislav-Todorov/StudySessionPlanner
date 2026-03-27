namespace StudySessionPlanner_App.Services.Contracts
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollUserAsync(int studySessionId, string userId);
        Task<bool> IsUserEnrolledAsync(int studySessionId, string userId);
        Task<int> GetEnrollmentCountAsync(int studySessionId);
    }
}