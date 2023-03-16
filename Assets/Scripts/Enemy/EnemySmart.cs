using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmart : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _forwardSpeed;

    [Header("Boundaries")]
    [SerializeField]
    private float _destroyHeight;

    [Header("Waypoints")]
    [SerializeField]
    private Vector3 _leftWallWaypoint;
    [SerializeField]
    private Vector3 _rightWallWaypoint;
    [SerializeField]
    private Vector3 _leftFloorWaypoint;
    [SerializeField]
    private Vector3 _rightFloorWaypoint;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserVolume;
    private AudioSource _audioSource;

    [Header("Misc")]
    [SerializeField]
    private int _killPoints;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private float _loiterTime;
    private bool _isMovingToWall;
    private bool _isOnWall;
    private bool _isHunting;

    // External Classes
    GameObject _player;
    UIManager _uiManager;

    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogWarning("Player Is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        _isMovingToWall = true;
        _isOnWall = false;
        _isHunting = false;
    }

    private void Update() 
    {
        if(_isMovingToWall)
        {
            MoveToWall();
        }
        else if (_isOnWall) 
        {
        MoveDownWall();
        }
        else if (_isHunting)
        {
            HuntForPlayer();
        }
        else
        {
            SelfDestruct();
        }
    }

    private void MoveToWall()
    {
        float _forwardMovement = _forwardSpeed * Time.deltaTime;
        Vector3 destination;

        if(this.transform.position.x <= 0) // Nearest to left wall
        {
            destination = _leftWallWaypoint;
        }
        else // Nearest to right wall
        {
            destination = _rightWallWaypoint;
        }

        transform.position = Vector3.MoveTowards(this.transform.position, destination, _forwardMovement);

        // Check if waypoint reached
        if (Vector3.Distance(this.transform.position, destination) < .1)
        {
            _isMovingToWall = false;
            _isOnWall = true;
        }
    }

    private void MoveDownWall()
    {
        float forwardMovement = _forwardSpeed * Time.deltaTime;
        Vector3 destination;

        if (this.transform.position.x <= 0) // Nearest to left wall
        {
            destination = _leftFloorWaypoint;
        }
        else // Nearest to right wall
        {
            destination = _rightFloorWaypoint;
        }

        transform.position = Vector3.MoveTowards(this.transform.position, destination, forwardMovement);

        // Check if waypoint reached
        if (Vector3.Distance(this.transform.position, destination) < .1)
        {
            _isOnWall = false;
            _isHunting = true;
            StartCoroutine(LoiterTimeRoutine());
        }
    }

    private void HuntForPlayer()
    {
        float forwardMovement = _forwardSpeed * Time.deltaTime;
        float xDistance = Mathf.Abs(this.transform.position.x - _player.transform.position.x);
        float yDistance = this.transform.position.y - _player.transform.position.y;   
        
       
        if(xDistance < .5f && yDistance <= -1.5f) // Enemy is underneath the player
        {            
            FireWeapon();
        }

        if (yDistance > -1.5f) // Player not high enough to get beneath
        {
            if(xDistance > 2.0f) // Player not about to ram enemy
            {
                if (this.transform.position.x <= _player.transform.position.x) // Enemy is left of player
                {
                    transform.position += Vector3.right * forwardMovement;
                }
                else // Enemy is right of player
                {
                    transform.position += Vector3.left * forwardMovement;
                }
            }
            else // Player is too close - enemy tries to avoid
            {
                if (this.transform.position.x <= _player.transform.position.x) // Enemy is left of player
                {
                    transform.position += Vector3.left * forwardMovement;
                }
                else // Enemy is right of player
                {
                    transform.position += Vector3.right * forwardMovement;
                }
            }
        }
        else // Player is high enough to get beneath
        {
            if (this.transform.position.x <= _player.transform.position.x) // Enemy is left of player
            {
                transform.position += Vector3.right * forwardMovement;
            }
            else // Enemy is right of player
            {
                transform.position += Vector3.left * forwardMovement;
            }
        }
    }

    private void SelfDestruct()
    {
        float forwardMovement = _forwardSpeed * Time.deltaTime;

        transform.position += Vector3.down * forwardMovement;

        if(this.transform.position.y < _destroyHeight)
        {
            Destroy(gameObject);
        }
    }

    private void FireWeapon() 
    {
        GameObject laser = Instantiate(_laserPrefab, this.transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_laserAudioClip, _laserVolume);
        _isHunting = false;
    }

    IEnumerator LoiterTimeRoutine()  // Enemy can only hunt for so long
    {
        yield return new WaitForSeconds(_loiterTime);
        _isHunting = false;
    }
}
