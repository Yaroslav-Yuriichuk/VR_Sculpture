namespace _Sculpture.Runtime.Auth
{
    public sealed class User
    {
        public string Id { get; set; }
        public string Email { get; }
        public string Password { get; }

        public User(string id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }
}