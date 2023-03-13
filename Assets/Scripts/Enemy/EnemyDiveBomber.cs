using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiveBomber : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _forwardSpeed;
    [SerializeField]
    private float _rotationSpeed;

    [Header("Path")]
    [SerializeField]
    private GameObject[] _easyPaths;
    [SerializeField]
    private GameObject[] _averagePaths;
    [SerializeField]
    private GameObject[] _difficultPaths;
    [SerializeField]
    private GameObject[] _waypoints;
    private int _currentPathID = 0;
    private int _currentWaypointID = 0;
    private GameObject[] _paths;

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
    private WaveManager _waveManager;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogWarning("Player Is Null!");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        if (_waveManager == null)
        {
            Debug.LogWarning("Wave Manager is Null!");
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
        // Select proper paths based on wave difficulty level
        if(_waveManager.GetWaveDifficulty() == "Easy")
        {
            _paths = _easyPaths;
        }
        else if (_waveManager.GetWaveDifficulty() == "Average")
        {
            _paths = _averagePaths;
        }
        else // Difficult
        {
            _paths = _difficultPaths;
        }

        // Select path
        _currentPathID =  Random.Range(0, _paths.Length);
        _waypoints = _paths[_currentPathID].GetComponent<PathData>().GetWayPoints();
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
