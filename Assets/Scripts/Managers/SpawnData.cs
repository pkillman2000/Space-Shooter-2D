using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour
{
    // Spawn Bondaries
    [SerializeField]
    Vector2 _upperLeftBoundary;
    [SerializeField]
    Vector2 _lowerRightBoundary;

    // Weighted random selection weight
    [SerializeField]
    private int _spawnWeight;

    public int GetSpawnWeight()
    {
        return _spawnWeight;
    }

    public Vector2 GetUpperLeftBoundary()
    {
        return _upperLeftBoundary;
    }

    public Vector2 GetLowerRightBoundary()
    {
        return _lowerRightBoundary;
    }
}
