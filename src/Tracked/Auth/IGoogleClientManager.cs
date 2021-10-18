using System.Threading.Tasks;
using Shared.Dtos;

namespace Tracked.Auth {
    public interface IGoogleClientManager {
        Task<GoogleResponse> LoginAsync();
        Task<GoogleResponse> SilentLoginAsync();
        void Logout();
        string IdToken { get; }
        GoogleUserDto CurrentUser { get; }
        bool IsLoggedIn { get; }
    }
}
