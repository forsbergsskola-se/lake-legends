public interface ICatchable
{
    public string Name { get; }
    public FloatRange RandomStopTimeRange { get; }
    public FloatRange RandomMoveTimeRange { get; }

    public float CatchableSpeed { get; }
    public float CatchableStrength { get; }
}