using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Spawn Enemy Info
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _minEnemySpawnTime;
    [SerializeField]
    private float _maxEnemySpawnTime;
    [SerializeField]
    private float _laserSpeed;
    [SerializeField]
    private float _enemySpeed;
    [SerializeField]
    private float _minEnemyFireRate;
    [SerializeField]
    private float _maxEnemyFireRate;

    private bool _canEnemySpawn = true;
    private int _enemyFighterWeight;
    private int _enemyBomberWeight;


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

    // Weighted Random Selection
    private int WeightedRandomSelection(GameObject[] prefabs)
    {
        int totalWeight = 0;
        int activeSum = 0;
        int randomWeight = 0;

        // Get total weight of various spawnable enemies
        for (int i = 0; i < prefabs.Length; i++)
        {
            if(prefabs[i].gameObject.name == "Enemy Fighter")
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemyFighterWeight);
            }
            else
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemyBomberWeight);
            }            
            totalWeight += prefabs[i].GetComponent<SpawnData>().GetSpawnWeight();

        }
        // Select random enemy based on weight
        randomWeight = Random.Range(0, totalWeight + 1);

        for (int i = 0; i < prefabs.Length; ++i)
        {
            activeSum += prefabs[i].GetComponent<SpawnData>().GetSpawnWeight();
            if (activeSum > randomWeight)
            {
                return i;
            }
        }
        return 0;
    }

    public void StartSpawning()
    {
        _canEnemySpawn = true;
        _canPowerUpSpawn = true;

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public void StopSpawning()
    {
        _canEnemySpawn = false;
        _canPowerUpSpawn = false;
    }

    // Spawn Enemies
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

    private void SpawnEnemy()
    {
        GameObject newEnemy;
        float spawnXPosition;
        float spawnYPosition;
        int randomEnemy;
        float leftBoundary;
        float rightBoundary;
        float upperBoundary;
        float lowerBoundary;

        randomEnemy = WeightedRandomSelection(_enemyPrefab);

        leftBoundary = _enemyPrefab[randomEnemy].GetComponent<SpawnData>().GetUpperLeftBoundary().x;
        rightBoundary = _enemyPrefab[randomEnemy].GetComponent<SpawnData>().GetLowerRightBoundary().x;
        upperBoundary = _enemyPrefab[randomEnemy].GetComponent<SpawnData>().GetUpperLeftBoundary().y;
        lowerBoundary = _enemyPrefab[randomEnemy].GetComponent<SpawnData>().GetLowerRightBoundary().y;

        spawnXPosition = Random.Range(leftBoundary, rightBoundary);
        spawnYPosition = Random.Range(lowerBoundary, upperBoundary);

        // Create reference to enemy that is instantiated, and then move it to the Enemy Container.  
        // This just makes the Hierarchy window cleaner.  It does not add any functionality to the game.
        newEnemy = Instantiate(_enemyPrefab[randomEnemy], new Vector3(spawnXPosition, spawnYPosition, 0), Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
        if(newEnemy.gameObject.name == "Enemy Fighter(Clone)")
        {
            newEnemy.GetComponent<Enemy>().SetLaserSpeed(_laserSpeed);
            newEnemy.GetComponent<Enemy>().SetMovementSpeed(_enemySpeed);
            newEnemy.GetComponent<Enemy>().SetFireRates(_minEnemyFireRate, _maxEnemyFireRate);
        }
    }

    // Change enemy stats for waves

    public void SetEnemySpawnTimes(float min, float max)
    {
        _minEnemySpawnTime = min;
        _maxEnemySpawnTime = max;
    }

    public void UpdateEnemyWeights(int fighterWeight, int bomberWeight)
    {
        _enemyFighterWeight = fighterWeight;
        _enemyBomberWeight = bomberWeight;
    }

    public void SetLaserSpeed(float speed)
    {
        _laserSpeed = speed;
    }

    public void SetEnemySpeed(float speed)
    {
        _enemySpeed = speed;
    }

    public void SetEnemyFireRates(float min, float max)
    {
        _minEnemyFireRate = min;
        _maxEnemyFireRate = max;
    }
    // Spawn Powerups
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

    private void SpawnPowerUp()
    {
        float spawnXPosition;
        float spawnYPosition;
        int randomPowerUp;
        float leftBoundary;
        float rightBoundary;
        float upperBoundary;
        float lowerBoundary;
        
        randomPowerUp = WeightedRandomSelection(_powerUpPrefab);

        leftBoundary = _powerUpPrefab[randomPowerUp].GetComponent<SpawnData>().GetUpperLeftBoundary().x;
        rightBoundary = _powerUpPrefab[randomPowerUp].GetComponent<SpawnData>().GetLowerRightBoundary().x;
        upperBoundary = _powerUpPrefab[randomPowerUp].GetComponent<SpawnData>().GetUpperLeftBoundary().y;
        lowerBoundary = _powerUpPrefab[randomPowerUp].GetComponent<SpawnData>().GetLowerRightBoundary().y;

        spawnXPosition = Random.Range(leftBoundary, rightBoundary);
        spawnYPosition = Random.Range(lowerBoundary, upperBoundary);

        Instantiate(_powerUpPrefab[randomPowerUp], new Vector3(spawnXPosition, spawnYPosition, 0), Quaternion.identity);
    }
}
