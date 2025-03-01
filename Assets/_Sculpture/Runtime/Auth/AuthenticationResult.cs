namespace _Sculpture.Runtime.Auth
{
    public sealed class AuthenticationResult
    {
        public User User { get; }



        public AuthenticationResult(User user)
        {
            User = user;
        }
    }
}