using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Speed and Movement
    [SerializeField]
    private float _horizontalSpeed;
    [SerializeField]
    private float _verticalSpeed;
    private float _horizontalMovement;
    private float _verticalMovement;
    [SerializeField]
    private float _vacuumSpeed;
    [SerializeField]
    private float _destroyHeight;
    private bool _vacuumActive;

    // Audio
    private AudioSource _audioSource;
    [SerializeField]
    [Range(0f, 1f)]
    private float _powerUpAudioVolume;
    [SerializeField]
    private AudioClip _powerUpAudioClip;

    [SerializeField]
    private int _spawnWeight;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpriteRenderer _spriteRenderer;
    private Engines _engine;
    private GameObject _player;
    private WaveManager _waveManager;

    private void Start()
    {
        _audioSource= GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogWarning("Sprite Renderer is Null!");
        }

        _engine = GameObject.Find("Player").GetComponent<Engines>();
        if(_engine == null)
        {
            Debug.LogWarning("Engines is Null!");
        }

        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogWarning("Player Is Null!");
        }

        _waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        if (_waveManager == null)
        {
            Debug.LogWarning("Wave Manager is Null");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C) && !_waveManager.GetPowerUpVacuumUsed()) 
        {
            _vacuumActive = true;
            _waveManager.SetPowerUpVacuumUsed(true);
        }

        CalculateMovement();
    }

    private void CalculateMovement()
    {
        _horizontalMovement = _horizontalSpeed * Time.deltaTime;
        _verticalMovement = -_verticalSpeed * Time.deltaTime;

        if(_vacuumActive)
        {
            float _forwardMovement = _vacuumSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _forwardMovement);
        }
        else
        {
            transform.Translate(new Vector3(_horizontalMovement, _verticalMovement, 0));

            if (transform.position.y < _destroyHeight)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Enemy Laser")
        {
            Destroy(collision.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        /*
        // Allow player to destroy powerup by accident?
        else if (other.transform.tag == "Laser")
        {
            Destroy(collision.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        */
    }
}