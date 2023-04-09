using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodge : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _verticalSpeed;
    [SerializeField]
    private float _horizontalSpeed;
    [SerializeField]
    private float _destroyHeight;
    private float _verticalMovement;
    private float _horizontalMovement;
    private float _leftBoundary;
    private float _rightBoundary;
    private float _spawnHeight;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserVolume;

    [Header("Weapons")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _minLaserFireTime;
    [SerializeField]
    private float _maxLaserFireTime;
    [SerializeField]
    private bool _canFire;

    [Header("Misc")]
    [SerializeField]
    private int _killPoints;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool _isDestroyed;

    // External Classes
    GameObject _player;
    UIManager _uiManager;
    SpawnData _spawnData;
    private AudioSource _audioSource;

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

        _spawnData = gameObject.GetComponent<SpawnData>();
        if (_spawnData == null)
        {
            Debug.LogWarning("Spawn Data is Null!");
        }
        else
        {
            _spawnHeight = _spawnData.GetUpperLeftBoundary().y;
            _leftBoundary = _spawnData.GetUpperLeftBoundary().x;
            _rightBoundary = _spawnData.GetLowerRightBoundary().x;
        }

        _isDestroyed = false;
        _canFire = true;

        StartCoroutine(FireLasers());
    }

    void Update()
    {
        CalculateVerticalMovement();
        DodgeLaser();
    }

    private void CalculateVerticalMovement()
    {
        _verticalMovement = -_verticalSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, _verticalMovement, 0));

        if (transform.position.y < _destroyHeight)
        {
            Respawn();
        }
    }

    private void DodgeLaser()
    {
        GameObject[] lasers;
        GameObject closest = null;
        float distance = Mathf.Infinity;

        lasers = GameObject.FindGameObjectsWithTag("Laser");

        foreach (GameObject laser in lasers) // Find closest laser
        {
            float curDistance = Mathf.Abs(laser.transform.position.x - transform.position.x);
            if(curDistance < 1) // Laser needs to be dodged
            {
                if (curDistance < distance)
                {
                    closest = laser;
                    distance = curDistance;
                }
            }
        }

        if(closest != null) 
        {
            if (closest.transform.position.x <= transform.position.x) // Laser is to left
            {
                _horizontalMovement = _horizontalSpeed * Time.deltaTime;
            }
            else // Laser is to right
            {
                _horizontalMovement = -_horizontalSpeed * Time.deltaTime;
            }

            transform.Translate(new Vector3(_horizontalMovement, 0, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isDestroyed)
        {
            if (other.transform.tag == "Player")
            {
                _uiManager.UpdateScore(_killPoints);
                DestroyMe();
            }
            else if ((other.transform.tag == "Laser") || (other.transform.tag == "Missile Homing"))
            {
                _uiManager.UpdateScore(_killPoints);

                Destroy(other.transform.gameObject);
                DestroyMe();
            }
        }
    }

    private IEnumerator FireLasers()
    {
        while (!_isDestroyed)
        {
            yield return new WaitForSeconds(Random.Range(_minLaserFireTime, _maxLaserFireTime));
            if (_canFire)
            {
                GameObject laser = Instantiate(_laserPrefab, this.transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_laserAudioClip, _laserVolume);
            }
        }
    }

    private void Respawn()
    {
        float spawnXPosition;

        spawnXPosition = Random.Range(_leftBoundary, _rightBoundary);
        transform.position = new Vector3(spawnXPosition, _spawnHeight, 0);
    }

    private void DestroyMe()
    {
        _isDestroyed = true;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
