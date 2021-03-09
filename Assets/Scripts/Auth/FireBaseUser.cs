using Firebase.Auth;

namespace Auth
{
    public class FireBaseUser : IUser
    {
        public FireBaseUser(IUserInfo firebaseUser)
        {
            ID = firebaseUser.UserId;
        }

        public string ID { get; }
    }
}