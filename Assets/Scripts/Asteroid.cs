using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private ScrollingBackground _scrollingBackground;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogWarning("Spawn Manager is Null!");
        }

        _scrollingBackground = GameObject.Find("Scrolling Background").GetComponent<ScrollingBackground>();
        if(_scrollingBackground == null)
        {
            Debug.LogWarning("Scrolling Background is Null!");
        }
    }


    void Update()
    {
        RotateAsteroid();
    }

    private void RotateAsteroid()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            _scrollingBackground.StartScrolling();
            Destroy(this.gameObject);
        }
    }
}
