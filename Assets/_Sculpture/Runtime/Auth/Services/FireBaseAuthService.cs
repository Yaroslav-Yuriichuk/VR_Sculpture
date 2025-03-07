using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;

namespace _Sculpture.Runtime.Auth.Services
{
    public sealed class FireBaseAuthService : IAuthService
    {
        private FirebaseAuth Auth => FirebaseAuth.DefaultInstance;

        public async UniTask<SignUpResult> SignUpAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                AuthResult authResult = await Auth.CreateUserWithEmailAndPasswordAsync(email, password)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                if (Auth.CurrentUser == null)
                {
                    return new SignUpResult(false, error: SignUpError.Unknown);
                }

                User user = new User(Auth.CurrentUser.UserId, Auth.CurrentUser.Email, password);
                SignUpResult result = new SignUpResult(true, user);

                return result;
            }
            catch (FirebaseException e)
            {
                AuthError error = (AuthError)e.ErrorCode;

                switch (error)
                {
                    case AuthError.EmailAlreadyInUse:
                        return new SignUpResult(false, error: SignUpError.EmailAlreadyInUse);
                    case AuthError.InvalidEmail:
                        return new SignUpResult(false, error: SignUpError.InvalidEmail);
                    case AuthError.WeakPassword:
                        return new SignUpResult(false, error: SignUpError.WeakPassword);
                    case AuthError.MissingEmail:
                        return new SignUpResult(false, error: SignUpError.MissingEmail);
                    case AuthError.MissingPassword:
                        return new SignUpResult(false, error: SignUpError.MissingPassword);
                    default:
                        return new SignUpResult(false, error: SignUpError.Unknown);
                }
            }
        }

        public async UniTask<SignInResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                AuthResult authResult = await Auth.SignInWithEmailAndPasswordAsync(email, password)
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

                if (Auth.CurrentUser == null)
                {
                    return new SignInResult(false, error: SignInError.Unknown);
                }

                User user = new User(Auth.CurrentUser.UserId, Auth.CurrentUser.Email, password);
                SignInResult result = new SignInResult(true, user);

                return result;
            }
            catch (FirebaseException e)
            {
                AuthError error = (AuthError)e.ErrorCode;

                switch (error)
                {
                    case AuthError.WrongPassword:
                        return new SignInResult(false, error: SignInError.WrongPassword);
                    case AuthError.UserNotFound:
                        return new SignInResult(false, error: SignInError.UserNotFound);
                    default:
                        return new SignInResult(false, error: SignInError.Unknown);
                }
            }
        }

        public void Logout()
        {
            Auth.SignOut();
        }
    }
}