using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootPowerup : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float _laserVolume;
    private AudioSource _audioSource;

    [Header("Weapons")]
    [SerializeField]
    private GameObject _laserPrefab;

    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("Audio Source is Null!");
        }

        ScanForPowerups();
    }

    public void ScanForPowerups()
    {
        GameObject[] powerups;
        powerups = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (GameObject powerup in powerups)
        {
            // See how far the powerup is from the fighter on the X axis
            float xDistance = Mathf.Abs(this.transform.position.x - powerup.transform.position.x);

            if (xDistance < .5f ) // Powerup is beneath the fighter
            {
                FireWeapon();
            }
        }
    }

    private void FireWeapon()
    {
        GameObject laser = Instantiate(_laserPrefab, this.transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_laserAudioClip, _laserVolume);
    }
}
