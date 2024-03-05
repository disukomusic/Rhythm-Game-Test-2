using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animation _anim;

    private void Awake()
    {
        _anim = GetComponent<Animation>();
    }

    public void PlayThisAnimation(AnimationClip anim)
    {
        _anim.clip = anim;
        _anim.Play();
    }
}
