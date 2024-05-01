using System;
using System.Collections;
using System.Collections.Generic;
using HDyar.OSUImporter;
using UnityEngine;
using UnityEngine.Serialization;

public class Note : MonoBehaviour
{
    public Color startColor;
    public Color endColor;

    public SpriteRenderer spriteRenderer;
    
    public float speed;
    public float angleOffset;
    public int noteStartTime;
    
    public int targetLane;
    
    private float _t;

    private Vector3 _targetPosition;
    private Vector3 _missedTargetPosition;

    private bool _isHittable;
    public bool isHoldable;

    public bool isHoldNote;
    public int holdNoteEndTime;
    public TrailRenderer holdNoteTrailRenderer;
    public float holdNoteHeldTime;
    private NoteHit _noteHit;

    private void Awake()
    {
        _noteHit = GetComponent<NoteHit>();
    }

    public void OnNoteSpawn()
    {
        holdNoteTrailRenderer.Clear();
        _t = 0;
        _targetPosition = CalculateTargetPosition();
        _missedTargetPosition = CalculateMissedTargetPosition();
        _isHittable = true;
        isHoldable = false;
        
        if (isHoldNote)
        {
            _noteHit.isHoldNote = true;
            holdNoteTrailRenderer.emitting = true;
            holdNoteHeldTime = (holdNoteEndTime - noteStartTime) / 1000f;
            holdNoteTrailRenderer.time = holdNoteHeldTime;
        }
        else
        {
            _noteHit.isHoldNote = false;
            holdNoteTrailRenderer.emitting = false;
        }
    }

    public void Update()
    {
        _t += Time.deltaTime;
        
        if (_t > 0f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = lookRotation;
            
            if (_isHittable)
            {
                spriteRenderer.color = Color.Lerp(endColor,startColor, Vector3.Distance(transform.position, _targetPosition)/GameManager.Instance.circleRadius);
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
            } else if (!_isHittable)
            {
                transform.position = Vector3.MoveTowards(transform.position, _missedTargetPosition, speed * Time.deltaTime);
            }
            
            if (Vector3.Distance(transform.position, _targetPosition ) < 0.01f)
            {
                _isHittable = false;
                isHoldable = true;
            }

            if (!isHoldNote)
            {
                if (Vector3.Distance(transform.position, _missedTargetPosition) < 0.01f)
                {
                    gameObject.SetActive(false);
                } 
            }
            
            if (isHoldNote && holdNoteEndTime < GameManager.Instance.accurateMusicTime)
            {
                isHoldable = false;
                holdNoteTrailRenderer.emitting = false;
                gameObject.SetActive(false);
            } 
        }
    }

    Vector3 CalculateTargetPosition() 
    {
        float angle = (360f / 8 * targetLane) + angleOffset;
        float radius = GameManager.Instance.circleRadius;
        
        Vector3 targetPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * radius;
        return targetPosition;
    }
    Vector3 CalculateMissedTargetPosition() 
    {
        float angle = (360f / 8 * targetLane) + angleOffset; 
        float radius = GameManager.Instance.circleRadius + 1;
        
        Vector3 targetPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f) * radius;
        return targetPosition;
    }
}