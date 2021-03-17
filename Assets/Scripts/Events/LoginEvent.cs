using Auth;

namespace Events
{
    public class LoginEvent
    {
        public readonly IUser User;
        public readonly bool Debug;

        public LoginEvent(IUser authorizer, bool debug)
        {
            User = authorizer;
            Debug = debug;
        }
    }
}