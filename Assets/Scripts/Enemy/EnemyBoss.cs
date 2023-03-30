using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    // Movement
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    Vector3 _waypoint;
    private bool _isMoving;

    // Health
    [SerializeField]
    private int _hitPoints;
    [SerializeField]
    private int _numberOfTurrets;
    [SerializeField]
    private bool _turretsDestroyed;
    [SerializeField]
    private GameObject _explosion;

    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null )
        {
            Debug.LogWarning("Audio Source not Found!");
        }

        _isMoving = true;
        _turretsDestroyed = false;
    }


    void Update()
    {
        // Move towards waypoint - stop when reached
        if(_isMoving) 
        {
            if (Vector3.Distance(transform.position, _waypoint) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _waypoint, _movementSpeed * Time.deltaTime);
            }
            else
            {
                _isMoving = false;
                _audioSource.Stop();
            }
        }
    }

    public void TurretDestroyed()
    {
        _numberOfTurrets--;
        if(_numberOfTurrets == 0) 
        {
            _turretsDestroyed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_turretsDestroyed && collision.tag == "Laser")
        {            
            if(_hitPoints <= 0) // Rig multiple explosions throughout the ship
            {
                Instantiate(_explosion, transform.position, Quaternion.identity);
                // UI Manager - Game Won
                Destroy(this.gameObject);
            }
            _hitPoints--;
        }
    }
}
