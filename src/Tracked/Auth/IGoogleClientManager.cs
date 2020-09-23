using System.Threading.Tasks;
using Shared.Dto;

namespace Tracked.Auth {
    public interface IGoogleClientManager {
        Task<GoogleResponse> LoginAsync();
        Task<GoogleResponse> SilentLoginAsync();
        void Logout();
        string IdToken { get; }
        string AccessToken { get; }
        GoogleUserDto CurrentUser { get; }
        bool IsLoggedIn { get; }
    }
}
