using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;

namespace _Sculpture.Runtime.Auth.Services
{
    public sealed class FirebaseAuthService : IAuthService, IDisposable
    {
        public event Action<User> UserSignedIn;
        public event Action<User, User> UserSwitched;
        public event Action<User> UserSignedOut;

        public User CurrentUser => _currentUser;

        private FirebaseAuth Auth => FirebaseAuth.DefaultInstance;

        private User _currentUser;

        public FirebaseAuthService()
        {
            _currentUser = FireBaseUserUtility.ToUser(Auth.CurrentUser);
            Auth.StateChanged += HandleAuthStateChanged;
        }

        public void Dispose()
        {
            Auth.StateChanged -= HandleAuthStateChanged;
        }

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

                User user = FireBaseUserUtility.ToUser(Auth.CurrentUser);
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

                User user = FireBaseUserUtility.ToUser(Auth.CurrentUser);
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

        public void SignOut()
        {
            Auth.SignOut();
        }

        private void HandleAuthStateChanged(object sender, EventArgs e)
        {
            User previousUser = _currentUser;
            User currentUser = FireBaseUserUtility.ToUser(Auth.CurrentUser);

            _currentUser = currentUser;

            if (currentUser == null && previousUser != null)
            {
                UserSignedOut?.Invoke(new User(previousUser.Id, previousUser.Email));
            }
            else if (currentUser != null && previousUser == null)
            {
                UserSignedIn?.Invoke(new User(currentUser.Id, currentUser.Email));
            }
            else if (currentUser != null && previousUser != null && currentUser.Id != previousUser.Id)
            {
                UserSwitched?.Invoke(new User(previousUser.Id, previousUser.Email), new User(currentUser.Id, currentUser.Email));
            }
        }
    }
}
