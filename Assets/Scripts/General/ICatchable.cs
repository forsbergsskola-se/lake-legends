using Items;
using UnityEngine;

public interface ICatchable
{
    public string Name { get; }
    public Sprite Sprite { get; }
    public Rarity Rarity { get; }
    public FloatRange RandomStopTimeRange { get; }
    public FloatRange RandomMoveTimeRange { get; }
    public float CatchableSpeed { get; }
    public float CatchableStrength { get; }
}