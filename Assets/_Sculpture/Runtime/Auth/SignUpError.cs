namespace _Sculpture.Runtime.Auth
{
    public enum SignUpError
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