using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Speed and Movement")]
    [SerializeField]
    private float _horizontalSpeed;
    [SerializeField] 
    private float _verticalSpeed;
    [SerializeField]
    private float _speedBoostSpeed;
    private float _horizontalMovement;
    private float _verticalMovement;

    [Header("Boundaries")]
    [SerializeField]
    private float _leftBoundary;
    [SerializeField]
    private float _rightBoundary;
    [SerializeField]
    private float _upperBoundary;
    [SerializeField]
    private float _lowerBoundary;
    [SerializeField]
    private bool _canWrap;

    [Header("Weapons")]
    [SerializeField]
    private GameObject _singleShotPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _weaponFireRate;
    [SerializeField]
    private string _fireKey;

    [Header("Shields")]
    [SerializeField]
    private GameObject _shieldPrefab;

    [Header("PowerUps")]
    private float _canFire = 0;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedBoostActive = false;
    [SerializeField]
    private bool _shieldsActive = false;
    [SerializeField]
    private float _powerDownTime;

    [Header("Damage")]
    [SerializeField]
    private int _lives;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject[] _damagePrefabs;

    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserAudioVolume;
    [SerializeField]
    private AudioClip _laserAudioClip;

    // External classes
    private SpawnManager _spawnManager;
    private ScrollingBackground _scrollingBackground;
    private UIManager _uiManager;

    void Start()
    {
        // Reset player position
        transform.position = new Vector3(0, -2, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogWarning("Spawn Manager is Null!");
        }

        _scrollingBackground = GameObject.Find("Scrolling Background").GetComponent<ScrollingBackground>();
        if(_scrollingBackground == null)
        {
            Debug.LogWarning("Scrolling Background is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null) 
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        // Ensure damage prefabs are not active
        _damagePrefabs[0].SetActive(false);
        _damagePrefabs[1].SetActive(false);
    }

    void Update()
    {
        CalculateMovement();
        FireWeapon();
    }

    private void CalculateMovement()
    {

        if (_speedBoostActive)
        {
            _horizontalMovement = Input.GetAxis("Horizontal") * _speedBoostSpeed * Time.deltaTime;

            _verticalMovement = Input.GetAxis("Vertical") * _speedBoostSpeed * Time.deltaTime;
        }
        else
        {
            _horizontalMovement = Input.GetAxis("Horizontal") * _horizontalSpeed * Time.deltaTime;

            _verticalMovement = Input.GetAxis("Vertical") * _verticalSpeed * Time.deltaTime;
        }
        
        transform.Translate(new Vector3(_horizontalMovement, _verticalMovement, 0));

        // If _canWrap is true, player moves off of one side of the screen and appears on the other side.
        // This only affects horizontal movement, not vertical.
        if (_canWrap)
        {
            // Check horizontal position vs boundaries
            if (transform.position.x < _leftBoundary)
            {
                transform.position = new Vector3(_rightBoundary, transform.position.y, 0);
            }
            else if (transform.position.x > _rightBoundary)
            {
                transform.position = new Vector3(_leftBoundary, transform.position.y, 0);
            }
        }
        else
        {
            transform.position = new Vector3( Mathf.Clamp(transform.position.x, _leftBoundary, _rightBoundary), transform.position.y, 0);
        }

        // Clamp vertical position to boundaries
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _lowerBoundary, _upperBoundary), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy Laser")
        {
            Damage();
        }
    }

    public void Damage()
    {
        if (!_shieldsActive) // Shields not active
        {
            _lives -= 1;
            _uiManager.UpdateLives(_lives);

            // Activate damage prefabs
            if(_lives == 2)
            { 
                _damagePrefabs[0].SetActive(true);
            }
            else if (_lives == 1)
            {
                _damagePrefabs[1].SetActive(true);
            }
            

            if (_lives <= 0) // Player dies
            {
                _spawnManager.StopSpawning();
                _scrollingBackground.StopScrolling();
                _uiManager.GameOver();

                Instantiate(_damagePrefabs[2], transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
        else // Shields active
        { 
            _shieldsActive = false; 
            _shieldPrefab.SetActive(false);
        }

    }

    private void FireWeapon()
    {
        if (Input.GetKeyDown(_fireKey) && Time.time > _canFire)
        {
            _canFire = Time.time + _weaponFireRate;

            if (_tripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_singleShotPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            }

            _audioSource.PlayOneShot(_laserAudioClip, _laserAudioVolume);
        }
    }

    // Triple Shot
    public void TripleShotActive()
    {
        _tripleShotActive = true; 
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTime);
        _tripleShotActive = false;
    }

    // Speed Boost
    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTime);
        _speedBoostActive = false;
    }

    // Shields
    public void ShieldsActive()
    {
        _shieldsActive = true;
        _shieldPrefab.SetActive(true);
    }

    // Score points comes from the enemy.  Some enemies may be 
    // worth more than others.
    public void AddToScore(int score)
    {
        _score += score;
        if(_uiManager!= null) 
        {
            _uiManager.UpdateScore(_score);
        }
    }
}
