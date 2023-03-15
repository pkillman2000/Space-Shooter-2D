using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRammer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _verticalSpeed;
    [SerializeField]
    private float _horizontalSpeed;
    private float _verticalMovement;
    private float _horizontalMovement;

    [Header("Misc")]
    [SerializeField]
    private float _destroyHeight;
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

    void Update()
    {
        CalculateVerticalMovement();

        CalculateHorizontalmovement();
    }

    // Movement
    private void CalculateVerticalMovement()
    {
        _verticalMovement = -_verticalSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, _verticalMovement, 0));

        if (transform.position.y < _destroyHeight)
        {
                Destroy(this.gameObject);
        }
    }

    private void CalculateHorizontalmovement() 
    {
        if(_player.transform.position.x <= this.transform.position.x) 
        { 
            _horizontalMovement = -_horizontalSpeed * Time.deltaTime;
        }
        else 
        {
            _horizontalMovement = _horizontalSpeed * Time.deltaTime;
        }

        transform.Translate(new Vector3(_horizontalMovement, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isDestroyed)
        {
            if (other.transform.tag == "Player")
            {
                _uiManager.UpdateScore(_killPoints);
                DestroyMe();
            }
            else if ((other.transform.tag == "Laser") || (other.transform.tag == "Missile Homing"))
            {
                _uiManager.UpdateScore(_killPoints);

                Destroy(other.transform.gameObject);
                DestroyMe();
            }
        }
    }

    private void DestroyMe()
    {
        _isDestroyed = true;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
