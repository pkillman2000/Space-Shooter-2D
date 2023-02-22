using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed;
    [SerializeField]
    private float _selfDestructHeight;
    [SerializeField]
    private bool _enemyLaser;

    void Update()
    {
        if(!_enemyLaser) // Player laser - bolt goes up, destructs at top of screen
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

            // Destroy after off screen
            if (transform.position.y > _selfDestructHeight)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

            if(transform.position.y < _selfDestructHeight)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
