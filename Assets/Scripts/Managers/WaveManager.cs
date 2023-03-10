using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Info")]
    [SerializeField]
    private float _waveLength;
    [SerializeField]
    private int _currentWaveID;
    [SerializeField]
    private float _pauseBetweenWaves;
    [SerializeField]
    private int _maxNumberOfWaves;
    [SerializeField]
    private int _easyWaveDifficulty;
    [SerializeField]
    private int _averageWaveDifficulty;
    [SerializeField]
    private string _currentWaveDifficulty;

    [Header("Enemy Spawn Info")]
    [SerializeField]
    private float[] _minEnemySpawnTime;
    [SerializeField]
    private float[] _maxEnemySpawnTime;
    [SerializeField]
    private int[] _enemyFighterWeight;
    [SerializeField]
    private int[] _enemyBomberWeight;

    [Header("Enemy Info")]
    [SerializeField]
    private float[] _enemyLaserSpeed;
    [SerializeField]
    private float[] _enemyFighterSpeed;
    [SerializeField]
    private float[] _minEnemyFighterFireRate;
    [SerializeField]
    private float[] _maxEnemyFighterFireRate;

    [Header("Spawn Info")]
    [SerializeField]
    private float[] _minSpawnTime;
    [SerializeField]
    private float[] _maxSpawnTime;

    private SpawnManager _spawnManager;
    private ScrollingBackground _scrollingBackground;
    private UIManager _uiManager;
    private Weapons _weapons;

    void Start()
    {
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

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _weapons = GameObject.Find("Player").GetComponent<Weapons>();
        if (_weapons == null) 
        {
            Debug.LogWarning("Weapons is Null!");
        }

        StartCoroutine(PauseBetweenWavesRoutine());
    }

    public void StartNewWave()
    {
        _spawnManager.StartSpawning();
        _scrollingBackground.StartScrolling();
        _weapons.ReloadAmmo();

        StartCoroutine(WaveTimerRoutine());
    }

    private void CalculateWaveDifficulties()
    {
        _easyWaveDifficulty = (int)Math.Round(_maxNumberOfWaves / 3f);
        _averageWaveDifficulty = (int)Math.Round(_maxNumberOfWaves / 1.5f);

        if (_currentWaveID <= _easyWaveDifficulty)
        {
            _currentWaveDifficulty = "Easy";
        }
        else if (_currentWaveID <= _averageWaveDifficulty)
        {
            _currentWaveDifficulty = "Average";
        }
        else
        {
            _currentWaveDifficulty = "Difficult";
        }
    }

    private IEnumerator WaveTimerRoutine()
    {
        yield return new WaitForSeconds(_waveLength);
        EndWave();
    }

    private void EndWave()
    {
        _spawnManager.StopSpawning();
        _scrollingBackground.StopScrolling();
        _uiManager.EndCurrentWave(_currentWaveID, 30, 20);

        DestroyAllEnemies();
        StartCoroutine(PauseBetweenWavesRoutine());
    }

    // Enemy fighters and dive bombers both have the tag
    // of Enemy.  All enemy weapons have the tag of Enemy Laser
    private void DestroyAllEnemies()
    {
        GameObject[] enemies;
        GameObject[] enemyWeapons;

        // Destroy all active enemies
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Destroy all active enemy weapons
        enemyWeapons = GameObject.FindGameObjectsWithTag("Enemy Laser");

       foreach (GameObject weapon in enemyWeapons)
       {
           Destroy(weapon);
       }
    }

    private IEnumerator PauseBetweenWavesRoutine()
    {
        yield return new WaitForSeconds(_pauseBetweenWaves);
        
        _currentWaveID++;
        CalculateWaveDifficulties();

        if (_currentWaveID <= _maxNumberOfWaves)
        {
            _uiManager.StartNewWave(_currentWaveID);
        }
        else
        {
            yield return new WaitForSeconds(_pauseBetweenWaves);
            _uiManager.GameOver(true);
        }
    }

    public float GetMinimumEnemySpawnRate()
    {
        return _minEnemySpawnTime[_currentWaveID - 1];
    }

    public float GetMaximumEnemySpawnRate() 
    {
        return (_maxEnemySpawnTime[_currentWaveID - 1]);
    }

    public int GetEnemyFighterWeight()
    {
        return _enemyFighterWeight[_currentWaveID - 1];
    }

    public int GetEnemyBomberWeight()
    {
        return _enemyBomberWeight[_currentWaveID - 1];
    }

    public float GetEnemyFighterMovementSpeed()
    {
        return _enemyFighterSpeed[_currentWaveID - 1];
    }

    public float GetEnemyLaserSpeed()
    {
        return _enemyLaserSpeed[_currentWaveID - 1];
    }

    public float GetMinEnemyFighterFireRate()
    {
        return _minEnemyFighterFireRate[_currentWaveID - 1];
    }

    public float GetMaxEnemyFighterFireRate()
    {
        return _maxEnemyFighterFireRate[_currentWaveID - 1];
    }

    public string GetWaveDifficulty()
    {
        return _currentWaveDifficulty;
    }

    public float GetMinSpawnTime()
    {
        return _minSpawnTime[_currentWaveID - 1];
    }

    public float GetMaxSpawnTime() 
    {
        return _maxSpawnTime[_currentWaveID - 1];
    }
}
