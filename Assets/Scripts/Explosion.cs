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
    }

    private void DestroyAnimation()
    {
        AnimationClip[] clipInfo;
        float animLength;

        clipInfo = _animator.runtimeAnimatorController.animationClips;
        animLength = clipInfo[0].length;

        Destroy(this.gameObject, animLength);
    }

}
