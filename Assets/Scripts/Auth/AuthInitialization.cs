using System.Collections;
using EventManagement;
using UnityEngine;

namespace Auth
{
    public class AuthInitialization : MonoBehaviour
    {
        public IUser CurrentUser { get; private set; }
        public bool debug = true;
        private IAuthorizer authorizer;
        private IEnumerator Start()
        {
            authorizer = debug ? (IAuthorizer) new DebugAuth() : new FirebaseAuthAdapter();
            yield return authorizer.Initialize();
            var userTask = authorizer.LoginAnonymously();
            while (!userTask.IsCompleted)
            {
                yield return null;
            }

            CurrentUser = userTask.Result;
            Debug.Log($"User {CurrentUser.ID} Logged In");
            FindObjectOfType<EventsBroker>().Publish(new LoginEvent(CurrentUser, debug));
        }

        private void OnDestroy()
        {
            authorizer.LogOut();
        }
    }

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
