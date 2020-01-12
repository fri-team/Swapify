using BlazorClient.Services.API;

namespace BlazorClient.Services
{
    public interface ISwapifyAPI
    {
        IUserEndpoints User { get; }
        IStudentEndpoints Student { get; }
        ITimetableEndpoints Timetable { get; }
        void SetAuthorizationToken(string token);
        void DeleteAuthorizationToken();
    }
}
