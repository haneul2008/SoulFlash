using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomValue
{
    public float GetRandom(float minValue, float maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
}
