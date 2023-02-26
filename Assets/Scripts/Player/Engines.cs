using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engines : MonoBehaviour
{
    [Header("Speed and Movement")]
    [SerializeField]
    private float _horizontalSpeed;
    [SerializeField]
    private float _verticalSpeed;
    [SerializeField]
    private float _speedBoostSpeed;
    private float _horizontalMovement;
    private float _verticalMovement;

    [Header("Boundaries")]
    [SerializeField]
    private float _leftBoundary;
    [SerializeField]
    private float _rightBoundary;
    [SerializeField]
    private float _upperBoundary;
    [SerializeField]
    private float _lowerBoundary;
    [SerializeField]
    private bool _canWrap;

    [Header("Particle Effects")]
    [SerializeField]
    private GameObject _normalEngineParticle;
    [SerializeField]
    private GameObject _boostedEngineParticle;

    [Header("Fuel")]
    [SerializeField]
    private float _maximumFuel;
    [SerializeField]
    private float _currentFuel;
    [SerializeField]
    private float _fuelFlowRate;

    // External classes
    private Player _player;
    private ScrollingBackground _scrollingBackground;
    private UIManager _uiManager;

    void Start()
    {
        // Reset player position
        transform.position = new Vector3(0, -2, 0);

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogWarning("Player is Null!");
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

        _normalEngineParticle.SetActive(true);
        _boostedEngineParticle.SetActive(false);

        _currentFuel = _maximumFuel;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        // Scroll background faster when thrusters are active.  Return to normal
        // speed when not active
        if(Input.GetKeyDown(KeyCode.LeftShift) && _currentFuel > 0)
        {
            _scrollingBackground.ThrustersActive();
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _scrollingBackground.ThrustersInactive();
        }

        if (Input.GetKey(KeyCode.LeftShift) && _currentFuel > 0)
        {
            _horizontalMovement = Input.GetAxis("Horizontal") * _speedBoostSpeed * Time.deltaTime;
            _verticalMovement = Input.GetAxis("Vertical") * _speedBoostSpeed * Time.deltaTime;

            _boostedEngineParticle.SetActive(true);

            _currentFuel -= _fuelFlowRate * Time.deltaTime;

            _uiManager.UpdateThrusterBar(_maximumFuel, _currentFuel);
        }
        else
        {
            _horizontalMovement = Input.GetAxis("Horizontal") * _horizontalSpeed * Time.deltaTime;

            _verticalMovement = Input.GetAxis("Vertical") * _verticalSpeed * Time.deltaTime;

            _boostedEngineParticle.SetActive(false);
        }

        transform.Translate(new Vector3(_horizontalMovement, _verticalMovement, 0));

        // If _canWrap is true, player moves off of one side of the screen and appears on the other side.
        // This only affects horizontal movement, not vertical.
        if (_canWrap)
        {
            // Check horizontal position vs boundaries
            if (transform.position.x < _leftBoundary)
            {
                transform.position = new Vector3(_rightBoundary, transform.position.y, 0);
            }
            else if (transform.position.x > _rightBoundary)
            {
                transform.position = new Vector3(_leftBoundary, transform.position.y, 0);
            }
        }
        else
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _leftBoundary, _rightBoundary), transform.position.y, 0);
        }

        // Clamp vertical position to boundaries
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _lowerBoundary, _upperBoundary), 0);
    }

    public void FuelPowerup()
    {
        _currentFuel = _maximumFuel;
        _uiManager.UpdateThrusterBar(_maximumFuel, _currentFuel);
    }

    // * Move horizontal
    // * Move vertical

    // * Boundaries

    // Normal Speed
        // -> Scrolling Background - Normal speed
        // * Hide boosted engine particle
        // * Play normal engine SFX

    // Boosted Speed (left shift)
        // Do we have fuel?
            // * Subtract Fuel
            // * -> UI Manager - Update Fuel
            // -> Scrolling Background - Boosted speed
            // * Show boosted engine particle
            // * Play boosted engine SFX

    // Add Fuel <- Player (powerup collision)
    // -> UI Manager - Update Fuel

}
