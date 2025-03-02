namespace _Sculpture.Runtime.Auth
{
    public enum RegistrationError
    {
        None,
        Unknown,
        EmailAlreadyInUse,
        InvalidEmail,
        WeakPassword,
        MissingEmail,
        MissingPassword,
    }
}