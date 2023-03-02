using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHoming : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed;
    [SerializeField]
    private float _missileLifetime;

    private GameObject _target;
    private float _missileStartTime;

    void Start()
    {
        _target = FindClosestEnemy();
        _missileStartTime = Time.time;
    }

    void Update()
    {
        if (_target != null)
        {
            if (Time.time < _missileStartTime + _missileLifetime)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _missileSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance) 
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
}
