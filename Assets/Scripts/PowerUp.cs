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
    /*
     * ID for powerups
     * 0 = Triple Shot
     * 1 = Speed
     * 2 = Shields
    */
    [SerializeField]
    private int _powerupID;

    // Audio
    private AudioSource _audioSource;
    [SerializeField]
    [Range(0f, 1f)]
    private float _powerUpAudioVolume;
    [SerializeField]
    private AudioClip _powerUpAudioClip;

    private SpriteRenderer _spriteRenderer;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            ScrollingBackground scrollingBackground = FindObjectOfType<ScrollingBackground>().GetComponent<ScrollingBackground>();
            float audioLength;

            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0: // Tripleshot
                        player.TripleShotActive();
                        break;
                    case 1: // Speed Boost
                        player.SpeedBoostActive();
                        if(scrollingBackground!= null)
                        {
                            scrollingBackground.SpeedBoostActive();
                        }
                        else
                        {
                            Debug.LogWarning("Scrolling Background Not Found!");
                        }
                        break;
                    case 2: // Shields
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.LogWarning("You did something silly with powerup IDs!!!");
                        break;
                }
            }

            _spriteRenderer.enabled = false;
           audioLength = _powerUpAudioClip.length;
            _audioSource.PlayOneShot(_powerUpAudioClip, _powerUpAudioVolume);
            Destroy(this.gameObject, audioLength);
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