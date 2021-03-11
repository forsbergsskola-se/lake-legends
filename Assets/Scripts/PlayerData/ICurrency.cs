namespace PlayerData
{
    public interface ICurrency
    {
        int Silver { get; }
        int Gold { get; }
        void Serialize();
        void Deserialize();
    }
}