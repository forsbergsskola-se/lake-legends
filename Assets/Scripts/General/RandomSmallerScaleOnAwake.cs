using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSmallerScaleOnAwake : MonoBehaviour
{
    public float minScaleMultiplier = 0.5f;
    private void Awake()
    {
        transform.localScale *= Random.Range(minScaleMultiplier, 1);
    }
}
