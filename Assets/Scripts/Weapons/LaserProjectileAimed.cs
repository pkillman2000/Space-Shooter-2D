using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectileAimed : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _selfDestructHeight;

    Rigidbody2D _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if( _rigidBody == null )
        {
            Debug.LogError("Rigid Body is Null!");
        }
    }


    void Update()
    {
        _rigidBody.velocity = transform.up * -_speed;

        if(this.transform.position.y < _selfDestructHeight )
        {
            Destroy(this.gameObject);
        }
    }
}
