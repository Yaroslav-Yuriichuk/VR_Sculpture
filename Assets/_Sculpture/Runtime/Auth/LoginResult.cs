namespace _Sculpture.Runtime.Auth
{
    public sealed class LoginResult
    {
        public bool IsSuccessful { get; }

        public User User { get; }

        public LoginError Error { get; }

        public LoginResult(bool isSuccessful, User user = null, LoginError error = LoginError.None)
        {
            IsSuccessful = isSuccessful;
            User = user;
            Error = error;
        }
    }
}