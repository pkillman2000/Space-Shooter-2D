using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
    private float _horizontalScrollSpeed;
    [SerializeField]
    private float _verticalScrollSpeed;
    [SerializeField]
    private float _verticalSpeedBoost;
    [SerializeField]
    private float _powerDownTime;
    [SerializeField]
    private bool _speedBoostActive = false;
    private MeshRenderer _meshRenderer;

    private bool _canScroll = false;
    public Vector2 currentOffset;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if(_meshRenderer == null)
        {
            Debug.LogWarning("MeshRenderer is null!");
        }

        StopScrolling();
    }

    private void Update()
    {
        ScrollBackground();
    }

    private void ScrollBackground()
    {
        float x = _horizontalScrollSpeed * Time.time;
        float y;
        if(_canScroll )
        {
            if (_speedBoostActive)
            {
                y = _verticalSpeedBoost * Time.time;
            }
            else
            {
                y = _verticalScrollSpeed * Time.time;
            }

            Vector2 offset = new Vector2(x, y);
            currentOffset = offset;

            if (_meshRenderer != null)
            {
                _meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
            }
        }
    }

    public void SetHorizontalScrollSpeed(float speed)
    {
        _horizontalScrollSpeed = speed;
    }

    public void SetVerticalScrollSpeed(float speed)
    {
        _verticalScrollSpeed = speed;
    }

    // This isn't really necessary, but it's an easy way to stop.
    public void StopScrolling()
    {
        _canScroll = false;
    }

    public void StartScrolling()
    {
        _canScroll = true;
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDownTime);
        _speedBoostActive = false;
    }

}
