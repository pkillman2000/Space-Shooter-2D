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
    private float _minEnemySpawnTime;
    private float _maxEnemySpawnTime;
    //private float _laserSpeed;
    //private float _enemyFighterSpeed;
    //private float _minEnemyFighterFireRate;
    //private float _maxEnemyFighterFireRate;

    private bool _canEnemySpawn = true;
    private int _enemyFighterWeight;
    private int _enemyBomberWeight;
    private int _enemyRammerWeight;
    private int _enemySmartWeight;
    private int _enemyDodgeWeight;

    // Spawn PowerUps Info
    [SerializeField]
    private GameObject[] _powerUpPrefab;
    private float _minPowerUpSpawnTime;
    private float _maxPowerUpSpawnTime;
    [SerializeField]
    private bool _canPowerUpSpawn = true;

    private WaveManager _waveManager;

    private void Start()
    {
        _waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        if(_waveManager == null ) 
        {
            Debug.LogWarning("Wave Manager is Null!");
        }
    }

    // Weighted Random Selection
    private int WeightedRandomSelection(GameObject[] prefabs)
    {
        int totalWeight = 0;
        int activeSum = 0;
        int randomWeight = 0;

        _enemyFighterWeight = _waveManager.GetEnemyFighterWeight();
        _enemyBomberWeight = _waveManager.GetEnemyBomberWeight();
        _enemyRammerWeight = _waveManager.GetEnemyRammerWeight();
        _enemySmartWeight = _waveManager.GetEnemySmartWeight();
        _enemyDodgeWeight = _waveManager.GetEnemyDodgeWeight();


        // Get total weight of various spawnable enemies
        for (int i = 0; i < prefabs.Length; i++)
        {
            if(prefabs[i].gameObject.name == "Enemy Fighter")
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemyFighterWeight);
            }
            else if (prefabs[i].gameObject.name == "Enemy Bomber")
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemyBomberWeight);
            } 
            else if (prefabs[i].gameObject.name == "Enemy Rammer")
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemyRammerWeight);
            }
            else if (prefabs[i].gameObject.name == "Enemy Smart")
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemySmartWeight);
            }
            else if (prefabs[i].gameObject.name == "Enemy Dodge")
            {
                prefabs[i].GetComponent<SpawnData>().SetSpawnWeight(_enemyDodgeWeight);
            }

            totalWeight += prefabs[i].GetComponent<SpawnData>().GetSpawnWeight();

        }
        // Select random enemy based on weight
        randomWeight = Random.Range(1, totalWeight + 1);

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

    public void SpawnPowerUps()
    {
        _canPowerUpSpawn = true;
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Spawn Enemies
    IEnumerator SpawnEnemyRoutine()
    {
        float currentSpawnTime;

        while (_canEnemySpawn) 
        {
            _minEnemySpawnTime = _waveManager.GetMinimumEnemySpawnRate();
            _maxEnemySpawnTime = _waveManager.GetMaximumEnemySpawnRate();

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
    }

    // Spawn Powerups
    IEnumerator SpawnPowerUpRoutine()
    {
        float currentSpawnTime;
        _minPowerUpSpawnTime = _waveManager.GetMinSpawnTime();
        _maxPowerUpSpawnTime += _waveManager.GetMaxSpawnTime();

        Debug.Log("Min/Max/Can Spawn: " + _minPowerUpSpawnTime + "/" + _maxPowerUpSpawnTime + "/" + _canPowerUpSpawn);

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
