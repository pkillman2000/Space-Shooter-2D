using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [Header("Lasers")]
    [SerializeField]
    private GameObject _singleShotPrefab;
    [SerializeField]
    private float _laserFireRate;
    [SerializeField]
    private int _maxLaserAmmo;
    [SerializeField]
    private int _currentLaserAmmo;
    [SerializeField]
    private bool _laserActive = true;
    private float _laserCanFireTime = 0;

    [Header("Tripleshot")]
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _tripleShotFireRate;
    [SerializeField]
    private float _tripleShotDuration;
    [SerializeField]
    private bool _tripleShotActive = false;
    private float _tripleShotCanFireTime = 0;

    [Header("Homing Missile")]
    [SerializeField]
    private GameObject _missileHomingPrefab;
    [SerializeField]
    private float _missileHomingFireRate;
    [SerializeField]
    private float _missileHomingDuration;
    [SerializeField]
    private bool _missileHomingActive = false;
    private float _missileHomingCanFireTime = 0;

    [Header("Audio")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserAudioVolume;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _tripleShotAudioVolume;
    [SerializeField]
    private AudioClip _tripleShotAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _missileHomingAudioVolume;
    [SerializeField]
    private AudioClip _missileHomingAudioClip;

    // External classes
    private UIManager _uiManager;
    private AudioSource _audioSource;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        _laserCanFireTime = Time.time;
        _tripleShotCanFireTime = Time.time;
        _missileHomingCanFireTime = Time.time;
    }

    void Update()
    {
        FireWeapon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PowerUp Triple Shot(Clone)")
        {
            TripleShotActive();
        }
        else if (collision.gameObject.name == "PowerUp Ammo(Clone)")
        {
            ReloadAmmo();
        }
        else if (collision.gameObject.name == "PowerUp Missile Homing(Clone)")
        {
            MissileHomingActive();
        }
    }

    private void FireWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_laserActive && Time.time > _laserCanFireTime) // Laser
            {
                if (_currentLaserAmmo > 0)
                {
                    Instantiate(_singleShotPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                    _audioSource.PlayOneShot(_laserAudioClip, _laserAudioVolume);
                    _currentLaserAmmo--;
                    _laserCanFireTime = Time.time + _laserFireRate;
                    _uiManager.UpdateAmmoBar(_maxLaserAmmo, _currentLaserAmmo);
                }
            }
            else if (_tripleShotActive && Time.time > _tripleShotCanFireTime) // Tripleshot
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_tripleShotAudioClip, _tripleShotAudioVolume);
                _tripleShotCanFireTime = Time.time + _tripleShotFireRate;
            }
            else if (_missileHomingActive && Time.time > _missileHomingCanFireTime) // Missile - Homing
            {
                Instantiate(_missileHomingPrefab, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_missileHomingAudioClip, _missileHomingAudioVolume);
                _missileHomingCanFireTime = Time.time + _missileHomingFireRate;
            }
        }
    }

    // Laser
    public void ReloadAmmo()
    {
        _currentLaserAmmo = _maxLaserAmmo;
        _uiManager.UpdateAmmoBar(_maxLaserAmmo, _currentLaserAmmo);
    }

    // Triple Shot
    public void TripleShotActive()
    {
        _laserActive = false;
        _tripleShotActive = true;
        _missileHomingActive = false;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        _laserActive = true;
        _tripleShotActive = false;
        _missileHomingActive = false;
    }

    // Missile Homing
    public void MissileHomingActive()
    {
        _laserActive = false;
        _tripleShotActive = false;
        _missileHomingActive = true;
        StartCoroutine(MissileHomingPowerDownRoutine());
    }

    IEnumerator MissileHomingPowerDownRoutine()
    {
        yield return new WaitForSeconds(_missileHomingDuration);
        _laserActive = true;
        _tripleShotActive= false;
        _missileHomingActive = false;
    }
}
