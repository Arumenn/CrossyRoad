using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Platform
{
    public GameObject platformPrefab;
    public float height;
    public PlatformType platformType;
}

public enum PlatformType
{
    GRASS = 1,
    WATER = 2,
    ROAD = 3,
    RAILROAD = 4
}