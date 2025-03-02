namespace _Sculpture.Runtime.Auth
{
    public sealed class RegistrationResult
    {
        public bool IsSuccessful { get; }

        public User User { get; }

        public RegistrationError Error { get; }

        public RegistrationResult(bool isSuccessful, User user = null, RegistrationError error = RegistrationError.None)
        {
            IsSuccessful = isSuccessful;
            User = user;
            Error = error;
        }
    }
}