using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FloatRange
{
    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
    [SerializeField] private float min = 0;
    [SerializeField] private float max = 1;

    public float Min => min;

    public float Max => max;

    public void Validate()
    {
        min = Mathf.Clamp(min, float.MinValue, max);
        max = Mathf.Clamp(max, min, float.MaxValue);
    }

    public float Randomize()
    {
        return Random.Range(min, max);
    }
}