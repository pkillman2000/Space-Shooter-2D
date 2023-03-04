using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathData : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _waypoints;

    public GameObject[] GetWayPoints()
    {
        return _waypoints;
    }
}
