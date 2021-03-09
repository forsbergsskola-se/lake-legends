namespace Auth
{
    public class DebugUser : IUser
    {
        public string ID { get; }

        public DebugUser()
        {
            ID = System.Guid.NewGuid().ToString();
        }
    }
}