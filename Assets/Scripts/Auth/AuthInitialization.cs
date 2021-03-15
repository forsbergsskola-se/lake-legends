using System.Collections;
using EventManagement;
using Firebase;
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
            var fireBaseInstance = FirebaseApp.GetInstance("https://lakelegends-ebdcd-default-rtdb.europe-west1.firebasedatabase.app/");
            FindObjectOfType<EventsBroker>().Publish(new LoginEvent(fireBaseInstance, CurrentUser, debug));
        }

        private void OnDestroy()
        {
            authorizer.LogOut();
        }
    }

    public class LoginEvent
    {
        public readonly FirebaseApp FirebaseApp;
        public readonly IUser User;
        public readonly bool Debug;

        public LoginEvent(FirebaseApp firebaseApp, IUser authorizer, bool debug)
        {
            FirebaseApp = firebaseApp;
            User = authorizer;
            Debug = debug;
        }
    }
}
