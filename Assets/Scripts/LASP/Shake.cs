using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    public float wiggleRange;
    public float speed;
    public AnimationCurve ease;
    
    private  Vector3 _currentPos;
    private float _t = 0;
    private Vector3 _originalPosition;
    private Vector3 _lerpPosition;
    private bool _isMoving;

    void Awake()
    {
        _originalPosition = transform.position;
        _lerpPosition = _originalPosition;
    }

    void Update()
    {
        _t += Time.deltaTime;
        
        if (!_isMoving && Vector3.Distance(transform.position, _lerpPosition) < 0.01f)
        {
            _lerpPosition = _originalPosition + GetRandomVector3(wiggleRange);
            _currentPos = transform.position;
            _t = 0f;
            _isMoving = true;
            MoveTo(_lerpPosition);
        }
        else if (_isMoving && Vector3.Distance(transform.position, _lerpPosition) < 0.01f)
        {
            _isMoving = false;
        }
        else if (_isMoving&& Vector3.Distance(transform.position, _lerpPosition) > 0.01f)
        {
            MoveTo(_lerpPosition);
        }
    }

    void MoveTo(Vector3 newPos)
    {
        transform.position = Vector3.Lerp(_currentPos, newPos, ease.Evaluate(_t / speed));
    }

    Vector3 GetRandomVector3(float range)
    {
        Vector3 randomVector3 = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        return randomVector3;
    }
}
