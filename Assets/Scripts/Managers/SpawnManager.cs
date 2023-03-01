using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Spawn Enemy Info
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _minEnemySpawnTime;
    [SerializeField]
    private float _maxEnemySpawnTime;
    private bool _canEnemySpawn = true;

    // Spawn PowerUps Info
    [SerializeField]
    private GameObject[] _powerUpPrefab;
    [SerializeField]
    private float _minPowerUpSpawnTime;
    [SerializeField]
    private float _maxPowerUpSpawnTime;
    private bool _canPowerUpSpawn = true;

    // Boundaries
    [SerializeField]
    private float _leftBoundary;
    [SerializeField]
    private float _rightBoundary;
    [SerializeField]
    private float _spawnHeight;

    IEnumerator SpawnEnemyRoutine()
    {
        float currentSpawnTime;

        while (_canEnemySpawn) 
        {
            SpawnEnemy();
            currentSpawnTime = Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime);
            yield return new WaitForSeconds(currentSpawnTime);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        float currentSpawnTime;

        while (_canPowerUpSpawn)
        {
            SpawnPowerUp();
            currentSpawnTime = Random.Range(_minPowerUpSpawnTime, _maxPowerUpSpawnTime);
            yield return new WaitForSeconds(currentSpawnTime);
        }
    }

    private void SpawnEnemy()
    {
        float spawnXPosition;
        GameObject newEnemy;
        spawnXPosition = Random.Range(_leftBoundary, _rightBoundary);
        // Create reference to enemy that is instantiated, and then move it to the Enemy Container.  
        // This just makes the Hierarchy window cleaner.  It does not add any functionality to the game.
        newEnemy = Instantiate(_enemyPrefab, new Vector3(spawnXPosition, _spawnHeight, 0), Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }

    private void SpawnPowerUp()
    {
        float spawnXPosition;
        int randomPowerUp;

        spawnXPosition = Random.Range(_leftBoundary, _rightBoundary);
        randomPowerUp = Random.Range(0, _powerUpPrefab.Length);
        Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(spawnXPosition, _spawnHeight, 0), Quaternion.identity);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public void StopSpawning()
    {
        _canEnemySpawn= false;
        _canPowerUpSpawn= false;
    }
}
