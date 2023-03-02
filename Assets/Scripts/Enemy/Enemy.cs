using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Speed and Movement
    [SerializeField]
    private float _horizontalSpeed;
    [SerializeField] 
    private float _verticalSpeed;
    private float _horizontalMovement;
    private float _verticalMovement;

    // Boundaries
    [SerializeField]
    private float _leftBoundary;
    [SerializeField]
    private float _rightBoundary;
    [SerializeField]
    private float _spawnHeight;
    [SerializeField]
    private float _destroyHeight;

    [SerializeField]
    private int _killPoints;

    private Player _player;
    private Animator _animator;
    private bool _isDestroyed = false;

    // Audio
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _explosionVolume;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserVolume;

    // Weapons
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _minLaserFireTime;
    [SerializeField]
    private float _maxLaserFireTime;
    [SerializeField]
    private bool _canFire;

    private UIManager _uiManager;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null )
        {
            Debug.LogWarning("Player Is Null!");
        }

        _animator = gameObject.GetComponent<Animator>();
        if(_animator == null ) 
        {
            Debug.LogWarning("Enemy Animator is Null!");
        }

        _audioSource= gameObject.GetComponent<AudioSource>();
        if(_audioSource == null ) 
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null )
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _canFire = true;
        StartCoroutine(FireLasers());
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        _horizontalMovement = _horizontalSpeed * Time.deltaTime;
        _verticalMovement = -_verticalSpeed * Time.deltaTime;
        transform.Translate(new Vector3(_horizontalMovement, _verticalMovement, 0));

        if (transform.position.y < _destroyHeight)
        {
            // If player is destroyed, do not respawn when off bottom of screen
            if(_player!= null && !_isDestroyed) 
            {
                Respawn();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator FireLasers()
    {
        while (!_isDestroyed)
        {
            yield return new WaitForSeconds(Random.Range(_minLaserFireTime, _maxLaserFireTime));
            if(_canFire)
            {
                Instantiate(_laserPrefab, this.transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_laserAudioClip, _laserVolume);
            }
        }
    }

    // This was created for testing purposes.  If the enemy moves off the bottom
    // of the screen, it is moved back to a spawning point. It is a possible game variation.
    private void Respawn()
    {
        float spawnXPosition;

        spawnXPosition = Random.Range(_leftBoundary, _rightBoundary);
        transform.position = new Vector3(spawnXPosition, _spawnHeight, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isDestroyed)
        {
            if (other.transform.tag == "Player")
            {                
                _uiManager.UpdateScore(_killPoints);
                _canFire = false;
                DestroyAnimation();
            }
            else if ((other.transform.tag == "Laser") || (other.transform.tag == "Missile Homing"))
            {               
                _uiManager.UpdateScore(_killPoints);
                _canFire = false;

                Destroy(other.transform.gameObject);
                DestroyAnimation();
            }
        }
    }

    private void DestroyAnimation()
    {
        AnimationClip[] clipInfo;
        float animLength;

        _isDestroyed = true;
        _horizontalSpeed = 0;
        _verticalSpeed = 0;
        _canFire = false;

        _animator.SetTrigger("OnEnemyDeath");

        clipInfo = _animator.runtimeAnimatorController.animationClips;
        animLength = clipInfo[0].length;

        _audioSource.PlayOneShot(_explosionAudioClip, _explosionVolume);

        Destroy(gameObject, animLength);
    }

}
