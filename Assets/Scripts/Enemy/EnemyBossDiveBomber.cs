using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossDiveBomber : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _forwardSpeed;
    [SerializeField]
    private float _rotationSpeed;

    [Header("Path")]
    [SerializeField]
    private GameObject _path;
    [SerializeField]
    private GameObject[] _waypoints;
    private int _currentWaypointID = 0;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _bombAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _bombVolume;
    private AudioSource _audioSource;

    [Header("Weapons")]
    [SerializeField]
    private GameObject _bombPrefab;
    [SerializeField]
    private float _minBombFireTime;
    [SerializeField]
    private float _maxBombFireTime;
    [SerializeField]
    private bool _canFire;

    [Header("Misc")]
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private int _killPoints;
    [SerializeField]
    private bool _isDestroyed = false;

    // External Classes
    private Player _player;
    private UIManager _uiManager;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogWarning("Player Is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if( _uiManager == null )
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        SelectPath();

        _canFire = true;
        StartCoroutine(DropBombs());
    }

    void Update()
    {
        // Select Path, then waypoints in the path
        CalculateMovement();
    }

    private void SelectPath()
    {
        _waypoints = _path.GetComponent<PathData>().GetWayPoints();
    }

    private void CalculateMovement()
    {
        if (Vector3.Distance(transform.position, _waypoints[_currentWaypointID].transform.position) < 0.01f)
        {
            if (_currentWaypointID < _waypoints.Length - 1)
            {
                _currentWaypointID++;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            MoveTowardsWaypoint();
        }
    }

    private void MoveTowardsWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypointID].transform.position, _forwardSpeed * Time.deltaTime);
    }

    private IEnumerator DropBombs()
    {
        while (!_isDestroyed)
        {
            yield return new WaitForSeconds(Random.Range(_minBombFireTime, _maxBombFireTime));
            if (_canFire)
            {
                Instantiate(_bombPrefab, this.transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_bombAudioClip, _bombVolume);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isDestroyed)
        {
            if (other.transform.tag == "Player")
            {
                _uiManager.UpdateScore(_killPoints);
                _canFire = false;
                DestroyMe();
            }
            else if ((other.transform.tag == "Laser") || (other.transform.tag == "Missile Homing"))
            {
                _uiManager.UpdateScore(_killPoints);
                _canFire = false;

                Destroy(other.transform.gameObject);
                DestroyMe();
            }
        }
    }

    private void DestroyMe()
    {
        _isDestroyed = true;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
