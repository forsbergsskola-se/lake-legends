using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace Auth
{
    public class FirebaseAuthAdapter : IAuthorizer
    {
        private FirebaseAuth firebaseAuth;

        private bool IsDebug => FirebaseApp.LogLevel == LogLevel.Debug;

        public async Task Initialize()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    InitializeAuthorization();
                    Debug.Log($"Analytics Init");
                } 
                else {
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + task.Result); ;
                }
            });
        }

        public async Task<IUser> LoginAnonymously()
        {
            var firebaseUserInfo = await Task.Run(() => firebaseAuth.SignInAnonymouslyAsync());
            return new FireBaseUser(firebaseUserInfo);
        }
        
        public void LogOut()
        {
            if (IsDebug)
                Debug.Log($"{firebaseAuth.CurrentUser} Logged Out");
            firebaseAuth.SignOut();
        }

        private void InitializeAuthorization()
        {
            firebaseAuth = FirebaseAuth.DefaultInstance;
            FirebaseApp.LogLevel = LogLevel.Debug;
        }
    }
}