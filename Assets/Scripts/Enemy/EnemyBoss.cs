using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    Vector3 _waypoint;
    private bool _isMoving;

    [Header("Health")]
    [SerializeField]
    private int _hitPoints;
    [SerializeField]
    private int _numberOfTurrets;
    [SerializeField]
    private bool _turretsDestroyed;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private int _killPoints;

    [Header("Spawning")]
    [SerializeField]
    private GameObject _bomberPrefab;
    [SerializeField]
    private Vector3 _spawnPosition;
    [SerializeField]
    private int _numberToSpawn;
    [SerializeField]
    private float _minSpawnTime;
    [SerializeField]
    private float _maxSpawnTime;
    [SerializeField]
    private float _bomberSpacingTime;
    [SerializeField]
    private bool _canSpawn;

    // External Classes
    private AudioSource _audioSource;
    private UIManager _uiManager;
    private WaveManager _waveManager;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null )
        {
            Debug.LogWarning("Audio Source not Found!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        if (_waveManager == null)
        {
            Debug.LogWarning("Wave Manager is Null");
        }

        _isMoving = true;
        _turretsDestroyed = false;
    }


    void Update()
    {
        // Move towards waypoint - stop when reached
        if(_isMoving) 
        {
            if (Vector3.Distance(transform.position, _waypoint) > 0.01f) // Still Moving
            {
                transform.position = Vector3.MoveTowards(transform.position, _waypoint, _movementSpeed * Time.deltaTime);
            }
            else // Done Moving
            {
                _isMoving = false;
                _audioSource.Stop();
                _canSpawn = true;
                StartCoroutine(SpawnRoutine());
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
                _uiManager.UpdateScore(_killPoints);
                _waveManager.DestroyAllEnemies();
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _uiManager.GameOver(true);
                Destroy(this.gameObject);
            }
            _hitPoints--;
        }
    }

    IEnumerator SpawnRoutine()
    {
        float currentSpawnTime;

        while (_canSpawn)
        {
            StartCoroutine(SpawnBombersRoutine());
            currentSpawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(currentSpawnTime);
        }
    }

    IEnumerator SpawnBombersRoutine()
    {
        int _counter = 0;
        
        while (_counter < _numberToSpawn)
        {
            Instantiate(_bomberPrefab, _spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(_bomberSpacingTime);
            _counter++;
        }
    }
}
