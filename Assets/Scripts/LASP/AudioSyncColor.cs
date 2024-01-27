using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from Renaissance Coders
//https://github.com/coderDarren/RenaissanceCoders_UnityScripting

//Example child class that adjusts color based on audio.
public class AudioSyncColor : AudioSyncer 
{
    public Color beatColor;
    public Color restColor;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //if it is not currently a beat, lerp back to the base color
        if (!m_isBeat)
        {
            _meshRenderer.material.color = Color.Lerp(_meshRenderer.material.color, restColor, restSmoothTime * Time.deltaTime);
        }
    }
    
    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", beatColor);
    }
    
    private IEnumerator MoveToColor(Color _target)
    {
        
        Color _curr = _meshRenderer.material.color;
        Color _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            _meshRenderer.material.color = _curr;

            yield return null;
        }

        m_isBeat = false;
    }
}