namespace _Sculpture.Runtime.Auth
{
    public sealed class SignInResult
    {
        public bool IsSuccessful { get; }

        public User User { get; }

        public SignInError Error { get; }

        public SignInResult(bool isSuccessful, User user = null, SignInError error = SignInError.None)
        {
            IsSuccessful = isSuccessful;
            User = user;
            Error = error;
        }
    }
}