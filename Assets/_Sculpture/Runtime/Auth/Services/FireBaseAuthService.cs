using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;

namespace _Sculpture.Runtime.Auth.Services
{
    public sealed class FireBaseAuthService : IAuthService
    {
        private FirebaseAuth Auth => FirebaseAuth.DefaultInstance;

        public async UniTask<RegistrationResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                AuthResult authResult = await Auth.CreateUserWithEmailAndPasswordAsync(email, password)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                if (Auth.CurrentUser == null)
                {
                    return new RegistrationResult(false, error: RegistrationError.Unknown);
                }

                User user = new User(Auth.CurrentUser.UserId, Auth.CurrentUser.Email, password);
                RegistrationResult result = new RegistrationResult(true, user);

                return result;
            }
            catch (FirebaseException e)
            {
                AuthError error = (AuthError)e.ErrorCode;

                switch (error)
                {
                    case AuthError.EmailAlreadyInUse:
                        return new RegistrationResult(false, error: RegistrationError.EmailAlreadyInUse);
                    case AuthError.InvalidEmail:
                        return new RegistrationResult(false, error: RegistrationError.InvalidEmail);
                    case AuthError.WeakPassword:
                        return new RegistrationResult(false, error: RegistrationError.WeakPassword);
                    case AuthError.MissingEmail:
                        return new RegistrationResult(false, error: RegistrationError.MissingEmail);
                    case AuthError.MissingPassword:
                        return new RegistrationResult(false, error: RegistrationError.MissingPassword);
                    default:
                        return new RegistrationResult(false, error: RegistrationError.Unknown);
                }
            }
        }

        public async UniTask<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                AuthResult authResult = await Auth.SignInWithEmailAndPasswordAsync(email, password)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                if (Auth.CurrentUser == null)
                {
                    return new LoginResult(false, error: LoginError.Unknown);
                }

                User user = new User(Auth.CurrentUser.UserId, Auth.CurrentUser.Email, password);
                LoginResult result = new LoginResult(true, user);

                return result;
            }
            catch (FirebaseException e)
            {
                AuthError error = (AuthError)e.ErrorCode;

                switch (error)
                {
                    case AuthError.WrongPassword:
                        return new LoginResult(false, error: LoginError.WrongPassword);
                    case AuthError.UserNotFound:
                        return new LoginResult(false, error: LoginError.UserNotFound);
                    default:
                        return new LoginResult(false, error: LoginError.Unknown);
                }
            }
        }

        public void Logout()
        {
            Auth.SignOut();
        }
    }
}