using System;

namespace ScoreService.Services
{
    public class SessionTokenStorageService : ISessionTokenStorageService
    {
        private Guid _sessionToken;
        public SessionTokenStorageService()
        {
            _sessionToken = Guid.NewGuid();
        }

        public void RefreshToken()
        {
            _sessionToken = Guid.NewGuid();
        }

        public string GetToken()
        {
            return _sessionToken.ToString();
        }
    }

    public interface ISessionTokenStorageService
    {
        void RefreshToken();
        string GetToken();
    }
}