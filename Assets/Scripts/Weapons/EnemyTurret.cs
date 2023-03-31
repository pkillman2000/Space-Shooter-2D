using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    // Rotate Turret Info
    private GameObject _player;
    Vector2 _targetDirection;
    float _targetAngle;

    [Header("Weapons")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _minLaserFireTime;
    [SerializeField]
    private float _maxLaserFireTime;
    [SerializeField]
    private bool _canFire;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserVolume;
    private AudioSource _audioSource;

    [Header("Damage")]
    [SerializeField]
    private int _hitPoints;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private int _killPoints;

    private EnemyBoss _enemyBoss;
    private UIManager _uiManager;

    private void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogWarning("Player Is Null!");
        }

        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        _enemyBoss = GameObject.Find("Enemy Boss(Clone)").GetComponent<EnemyBoss>();
        if (_enemyBoss == null)
        {
            Debug.LogWarning("Enemy Boss is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null) 
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _canFire = true;
        StartCoroutine(FireLasers());
    }

    private void Update()
    {
        if (_player != null)
        {
            RotateTurret();
        }
        else
        {
            _canFire = false;
        }
    }

    private void RotateTurret()
    {
            _targetDirection = _player.transform.position - this.transform.position;
            _targetAngle = (Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg) + 90;
            transform.rotation = Quaternion.Euler(0f, 0f, _targetAngle);
    }

    private IEnumerator FireLasers()
    {
        while(_canFire) 
        {
            yield return new WaitForSeconds(Random.Range(_minLaserFireTime, _maxLaserFireTime));
            GameObject laser = Instantiate(_laserPrefab, this.transform.position, transform.rotation);
            _audioSource.PlayOneShot(_laserAudioClip, _laserVolume);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            _hitPoints--;
            Destroy (collision.gameObject);
            if(_hitPoints == 0 )
            {
                _uiManager.UpdateScore(_killPoints);
                Instantiate (_explosion, this.transform.position, Quaternion.identity);
                _enemyBoss.TurretDestroyed();
                Destroy(this.gameObject);
            }
        }
    }
}
