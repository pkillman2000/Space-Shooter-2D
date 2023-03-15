using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shields : MonoBehaviour
{
    [Header("Shields")]
    [SerializeField]
    private GameObject[] _shields;
    [SerializeField]
    private int _shieldActive;
    [SerializeField]
    private bool _areShieldsActive;

    [Header("Shields/Damage")]
    [SerializeField]
    private GameObject[] _damagePrefabs;
    [SerializeField]
    private int _lives;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private ScrollingBackground _scrollingBackground;
    private CameraShake _cameraShake;


    void Start()
    {
        ResetAllShields();
        _shieldActive = -1;

        // turn off all damage
        DisplayDamage();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogWarning("Spawn Manager is Null!");
        }

        _scrollingBackground = GameObject.Find("Scrolling Background").GetComponent<ScrollingBackground>();
        if (_scrollingBackground == null)
        {
            Debug.LogWarning("Scrolling Background is Null!");
        }

        _cameraShake = GameObject.Find("Camera").GetComponent<CameraShake>();
        if (_cameraShake == null) 
        {
            Debug.LogWarning("CameraShake is Null!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Enemy Laser")
        {
            DecreaseShields();
        }
        else if (collision.tag == "Shield Powerup")
        {
            IncreaseShields();
        }
        else if (collision.tag == "Health Powerup")
        {
            CureDamage();
        }
        else if (collision.tag == "Remove Shield Powerup")
        {
            ResetAllShields();
        }
    }

    public void IncreaseShields()
    {
        if(_shieldActive < _shields.Length - 1)
        {
            ResetAllShields();
            _shieldActive++;

            _shields[_shieldActive].SetActive(true);
            _areShieldsActive = true;
        }
    }

    private void DecreaseShields()
    {
        if (!_areShieldsActive) // No shields
        {
            TakeDamage();
        }
        else
        {
            ResetAllShields();
            _shieldActive--;
            _cameraShake.ShakeCamera();
            Debug.Log("Shield Active: " + _shieldActive);
            
            if (_shieldActive >= 0) // Shields still active
            {
                _shields[_shieldActive].SetActive(true);
                _areShieldsActive = true;
            }
            else // Last shield drained
            {
                _shieldActive = -1;
                _areShieldsActive = false;
            }
        }
    }

    private void ResetAllShields()
    {
        // turn off all shields
        for (int i = 0; i < _shields.Length; i++)
        {
            _shields[i].SetActive(false);
        }
    }

    private void TakeDamage()
    {
        _lives -= 1;
        DisplayDamage();
        _cameraShake.ShakeCamera();
    }

    private void CureDamage()
    {
        if(_lives < 3)
        {
            _lives++;
            DisplayDamage();
        }
    }

    private void DisplayDamage()
    {
        // Activate damage prefabs
        if(_lives == 3)
        {
            _damagePrefabs[0].SetActive(false);
            _damagePrefabs[1].SetActive(false);
        }
        else if (_lives == 2)
        {
            _damagePrefabs[0].SetActive(true);
            _damagePrefabs[1].SetActive(false);
        }
        else if (_lives == 1)
        {
            _damagePrefabs[1].SetActive(true);
            _damagePrefabs[0].SetActive(true);
        }
        else if (_lives <= 0) // Player dies
        {
            _spawnManager.StopSpawning();
            _scrollingBackground.StopScrolling();
            _uiManager.GameOver(false);

            Instantiate(_damagePrefabs[2], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
