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
    private float _destroyHeight;

    // Audio
    private AudioSource _audioSource;
    [SerializeField]
    [Range(0f, 1f)]
    private float _powerUpAudioVolume;
    [SerializeField]
    private AudioClip _powerUpAudioClip;

    [SerializeField]
    private int _spawnWeight;   

    private SpriteRenderer _spriteRenderer;
    private Engines _engine;

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
    }

    private void Update()
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
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        /*
        // Allow player to destroy powerup by accident?
        if (other.transform.tag == "Laser")
        {
            Destroy(other.transform.gameObject);
            Destroy(this.gameObject);
        }
        */
    }
}