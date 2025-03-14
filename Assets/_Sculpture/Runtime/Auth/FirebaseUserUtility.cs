using Firebase.Auth;

namespace _Sculpture.Runtime.Auth
{
    internal static class FireBaseUserUtility
    {
        public static User ToUser(FirebaseUser user)
        {
            return user != null ? new User(user.UserId, user.Email) : null;
        }
    }
}