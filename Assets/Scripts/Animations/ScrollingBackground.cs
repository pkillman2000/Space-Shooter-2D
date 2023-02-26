using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // I included variables for horizontal scrolling
    // but they are not used in this project.
    [SerializeField]
    private float _horizontalScrollSpeed;
    [SerializeField]
    private float _verticalScrollSpeed;
    [SerializeField]
    private float _horizontalSpeedThrusters;
    [SerializeField]
    private float _verticalSpeedThrusters;
    [SerializeField]
    private bool _thrustersActive = false;

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
        float x;
        float y;
        if(_canScroll )
        {
            if (_thrustersActive)
            {
                y = _verticalSpeedThrusters * Time.time;
                x= _horizontalSpeedThrusters * Time.time;
            }
            else
            {
                y = _verticalScrollSpeed * Time.time;
                x = _horizontalScrollSpeed * Time.time;
            }

            Vector2 offset = new Vector2(x, y);
            currentOffset = offset;

            if (_meshRenderer != null)
            {
                _meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
            }
        }
    }


    // I could just pass in a bool, but it's easier
    // in other scripts to have separate methods
    public void StopScrolling()
    {
        _canScroll = false;
    }

    public void StartScrolling()
    {
        _canScroll = true;
    }

    public void ThrustersActive()
    {
        _thrustersActive = true;
    }

    public void ThrustersInactive()
    {
        _thrustersActive = false;
    }
}
