namespace Saving
{
    public readonly struct CurrencySave
    {
        public readonly int Silver;
        public readonly int Gold;

        public CurrencySave(int silver, int gold)
        {
            Silver = silver;
            Gold = gold;
        }
    }
}