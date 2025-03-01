using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Sculpture.Runtime.Auth.Services
{
    public interface IAuthService
    {
        UniTask<RegistrationResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default);

        UniTask<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

        void Logout();
    }
}