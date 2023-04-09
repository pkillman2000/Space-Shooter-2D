using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogWarning("Animator is Null!");
        }

        DestroyAnimation();
    }

    private void DestroyAnimation()
    {
        AnimationClip[] clipInfo;
        float animLength;

        clipInfo = _animator.runtimeAnimatorController.animationClips;
        animLength = clipInfo[0].length;

        // Don't destroy game object until animation is complete
        Destroy(gameObject, animLength);
    }

}
