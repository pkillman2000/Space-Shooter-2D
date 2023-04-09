using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed;
    [SerializeField]
    private float _selfDestructHeight;
    [SerializeField]
    private bool _moveDownward; // Lets laser be used for Player or Enemy

    private WaveManager _waveManager;

    private void Start()
    {
        _waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        if (_waveManager == null)
        {
            Debug.LogWarning("Wave Manager is Null!");
        }

        if(_moveDownward) 
        {
            _laserSpeed = _waveManager.GetEnemyLaserSpeed();
        }
    }

    void Update()
    {
        if(!_moveDownward) // Player laser - bolt goes up, destructs at top of screen
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

            // Destroy after off screen
            if (transform.position.y > _selfDestructHeight)
            {
                Destroy(this.gameObject);
            }
        }
        else // Enemy laser - bolt goes down, destructs at bottom of screen
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

            if(transform.position.y < _selfDestructHeight)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetLaserSpeed(float speed)
    {
        _laserSpeed = speed;
    }
}
