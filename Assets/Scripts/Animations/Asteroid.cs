using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
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



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            _scrollingBackground.StartScrolling();
            Destroy(gameObject);
        }
    }
}
