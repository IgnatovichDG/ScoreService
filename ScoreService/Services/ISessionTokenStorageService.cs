namespace ScoreService.Services
{
    public interface ISessionTokenStorageService
    {
        void RefreshToken();
        string GetToken();
    }
}