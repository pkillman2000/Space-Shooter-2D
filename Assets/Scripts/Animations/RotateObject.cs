using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;

    void Update()
    {
        RotateGameObject();
    }

    private void RotateGameObject()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
