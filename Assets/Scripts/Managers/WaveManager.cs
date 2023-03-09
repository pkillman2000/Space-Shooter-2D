using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Info")]
    [SerializeField]
    private float _waveLength;
    [SerializeField]
    private int _currentWaveID = 0;
    [SerializeField]
    private float _pauseBetweenWaves;
    [SerializeField]
    private int _maxNumberOfWaves;

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
    private float[] _enemySpeed;
    [SerializeField]
    private float[] _minEnemyFireRate;
    [SerializeField]
    private float[] _maxEnemyFireRate;


    private SpawnManager _spawnManager;
    private ScrollingBackground _scrollingBackground;
    private UIManager _uiManager;

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

        StartCoroutine(PauseBetweenWavesRoutine());
    }

    public void StartNewWave()
    {
        _spawnManager.StartSpawning();
        _scrollingBackground.StartScrolling();

        // Increase Difficulties
        if (_currentWaveID < _maxNumberOfWaves)
        {
            UpdateEnemySpawnTimes();
            UpdateEnemyRandomSelectionWeights();
            UpdateEnemyLaserSpeed();
            UpdateEnemySpeed();
            UpdateEnemyFireRate();
        }

        StartCoroutine(WaveTimerRoutine());
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
        if(_currentWaveID < _maxNumberOfWaves)
        {
            _currentWaveID++;
            _uiManager.StartNewWave(_currentWaveID);
        }
        else
        {
            yield return new WaitForSeconds(_pauseBetweenWaves);
            _uiManager.GameOver(true);
        }
    }

    // Update Difficulties
    private void UpdateEnemySpawnTimes()
    {
        _spawnManager.SetEnemySpawnTimes(_minEnemySpawnTime[_currentWaveID], _maxEnemySpawnTime[_currentWaveID]);
    }

    private void UpdateEnemyRandomSelectionWeights()
    {
        _spawnManager.UpdateEnemyWeights(_enemyFighterWeight[_currentWaveID], _enemyBomberWeight[_currentWaveID]);
    }

    private void UpdateEnemyLaserSpeed()
    {
        _spawnManager.SetLaserSpeed(_enemyLaserSpeed[_currentWaveID]);
    }

    private void UpdateEnemySpeed()
    {
        _spawnManager.SetEnemySpeed(_enemySpeed[_currentWaveID]);
    }

    private void UpdateEnemyFireRate()
    {
        _spawnManager.SetEnemyFireRates(_minEnemyFireRate[_currentWaveID], _maxEnemyFireRate[_currentWaveID]);
    }
}
