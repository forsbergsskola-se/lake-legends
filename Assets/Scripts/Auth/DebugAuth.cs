using System.Threading.Tasks;

namespace Auth
{
    public class DebugAuth : IAuthorizer
    {
        public Task Initialize()
        {
            return new Task(TestFunction);
        }

        public async Task<IUser> LoginAnonymously()
        {
            var task = await Task.Run(CreateDebugUser);
            return task;
        }

        private async void TestFunction()
        {
            await Task.Delay(1);
        }

        private IUser CreateDebugUser()
        {
            return new DebugUser();
        }

        public void LogOut()
        {
            
        }
    }
}