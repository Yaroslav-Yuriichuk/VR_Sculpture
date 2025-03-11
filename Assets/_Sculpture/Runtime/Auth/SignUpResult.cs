namespace _Sculpture.Runtime.Auth
{
    public sealed class SignUpResult
    {
        public bool IsSuccessful { get; }

        public User User { get; }

        public SignUpError Error { get; }

        public SignUpResult(bool isSuccessful, User user = null, SignUpError error = SignUpError.None)
        {
            IsSuccessful = isSuccessful;
            User = user;
            Error = error;
        }
    }
}