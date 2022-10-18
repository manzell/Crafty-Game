using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Distribution
{
    public float mean, deviation; 

    public float Generate()
    {
        float u1 = 1f - UnityEngine.Random.value;
        float u2 = 1f - UnityEngine.Random.value;
        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * MathF.PI * u2);
        return mean + randStdNormal * deviation;
    }

    public static float Generate(Distribution d) => d.Generate(); 
}
