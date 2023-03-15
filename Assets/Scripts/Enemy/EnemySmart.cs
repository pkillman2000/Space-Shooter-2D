using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmart : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _verticalSpeed;
    [SerializeField]
    private float _horizontalSpeed;
    private float _verticalMovement;
    private float _horizontalMovement;
    [SerializeField]
    private float _destroyHeight;
    [SerializeField]
    private float _bottomOfScreen;

    [Header("Misc")]
    [SerializeField]
    private int _killPoints;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool _isDestroyed;

    // External Classes
    GameObject _player;
    UIManager _uiManager;

    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogWarning("Player Is Null!");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UI Manager is Null!");
        }

        _isDestroyed = false;
    }
}
