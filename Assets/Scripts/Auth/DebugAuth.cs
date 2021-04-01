using System.Threading.Tasks;

namespace Auth
{
    public class DebugAuth : IAuthorizer
    {
        public async Task Initialize()
        {
            await TestFunction();
        }

        public async Task<IUser> LoginAnonymously()
        {
            var task = await CreateDebugUser();
            return task;
        }

        private async Task TestFunction()
        {
            await Task.Delay(1);
        }

        private Task<IUser> CreateDebugUser()
        {
            return Task.FromResult(new DebugUser() as IUser);
        }

        public void LogOut()
        {
            
        }
    }
}