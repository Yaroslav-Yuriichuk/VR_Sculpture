using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Sculpture.Runtime.Auth.Services
{
    /// <summary>
    /// Interface for an auth service managing authentication.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the specified email and password.
        /// </summary>
        /// <param name="email">The email of the user to register.</param>
        /// <param name="password">The password of the user to register.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the registration operation.</returns>
        UniTask<SignUpResult> SignUpAsync(string email, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Logs in a user with the specified email and password.
        /// </summary>
        /// <param name="email">The email of the user to log in.</param>
        /// <param name="password">The password of the user to log in.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the login operation.</returns>
        UniTask<SignInResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        void Logout();
    }
}