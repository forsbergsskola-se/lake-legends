using System.Threading.Tasks;

namespace Auth
{
    public interface IAuthorizer
    {
        Task Initialize();

        Task<IUser> LoginAnonymously();

        void LogOut();
    }
}