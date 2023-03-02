using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _shakeAmplitude; // How much shake
    [SerializeField]
    private float _shakeDuration; // How long it lasts
    [SerializeField]
    private float _shakeDecrease; // How much it dies down
    
    private Camera _camera;
    private Vector3 _originalPosition;
    private bool _shakeActive = false;
    private float _remainingShakeDuration = 0;

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null )
        {
            Debug.LogWarning("Camera is Null!");
        }
        else
        {
            _originalPosition = _camera.transform.position;
        }
    }

    void Update()
    {
        if(_shakeActive) 
        {
            if(_remainingShakeDuration > 0) 
            {
                _remainingShakeDuration -= Time.deltaTime * _shakeDecrease;
                _camera.transform.localPosition = _originalPosition + Random.insideUnitSphere * _shakeAmplitude;
            }
            else
            {
                _shakeActive = false;
                _remainingShakeDuration = 0;
                _camera.transform.localPosition = _originalPosition;
            }
        }
    }

    public void ShakeCamera()
    {
        _remainingShakeDuration = _shakeDuration;
        _shakeActive = true;
    }
}
