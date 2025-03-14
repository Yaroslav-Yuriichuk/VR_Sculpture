namespace _Sculpture.Runtime.Auth
{
    public sealed class User
    {
        public string Id { get; set; }
        public string Email { get; }

        public User(string id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}