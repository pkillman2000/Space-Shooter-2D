using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField]
    private GameObject _singleShotPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _weaponFireRate;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private int _currentAmmo;

    [Header("PowerUps")]
    private float _canFireTime = 0;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private float _powerDownTime;

    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserAudioVolume;
    [SerializeField]
    private AudioClip _laserAudioClip;

    // External classes
    private UIManager _uiManager;

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
    }

    void Update()
    {
        FireWeapon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tripleshot Powerup")
        {
            TripleShotActive();
        }
        else if (collision.tag == "Ammo Powerup")
        {
            ReloadAmmo();
        }
    }

    private void FireWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFireTime)
        {
            _canFireTime = Time.time + _weaponFireRate;

            if (_tripleShotActive) // Triple Shot does not use ammo
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_laserAudioClip, _laserAudioVolume);
            }
            else // Normal laser uses ammo
            {
                if(_currentAmmo > 0)
                {
                    Instantiate(_singleShotPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
                    _audioSource.PlayOneShot(_laserAudioClip, _laserAudioVolume);

                    _currentAmmo--;
                    _uiManager.UpdateAmmoBar(_maxAmmo, _currentAmmo);
                }
            }
        }
    }

    private void ReloadAmmo()
    {
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmoBar(_maxAmmo, _currentAmmo);
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

    // Missile
}
