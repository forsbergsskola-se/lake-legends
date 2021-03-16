public interface ICatchable
{
    public FloatRange RandomStopTimeRange { get; }
    public FloatRange RandomMoveTimeRange { get; }

    public float CatchableSpeed { get; }
    public float CatchableStrength { get; }
}