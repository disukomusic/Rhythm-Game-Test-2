using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.OSUImporter;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private OSUParser parser;
    [SerializeField] private NoteSpawner noteSpawner;
    
    public float accuracy;
    public float timingWindow;
    public float perfectMargin;
    public float score;
    
    
    [SerializeField] private OSUHitObject[] currentHitObjects;
    private int _laneInput;
    private KeyCode[] inputs = 
        new []
        {
            KeyCode.Alpha1 , KeyCode.Alpha2, KeyCode.Alpha3 , KeyCode.Alpha4,
            KeyCode.Alpha5 , KeyCode.Alpha6, KeyCode.Alpha7 , KeyCode.Alpha8
        };

    private bool isHeld;
    private Coroutine holdRoutine;

    private void Update()
    {
        foreach (OSUHitObject hitObject in currentHitObjects)
        { 
            if (hitObject.EndTime == -1) //Normal notes have -1 end time
            {
                if (Input.GetKeyDown(inputs[GetRealLaneNumber(hitObject.X)]))
                {
                    int inputDownTime = (int)GameManager.Instance.accurateMusicTime;
                    int inputVariation = Mathf.Abs(hitObject.Time - inputDownTime);
                
                    if (inputVariation  <= timingWindow)
                    {
                        if (inputDownTime > hitObject.Time && inputVariation > perfectMargin)
                        {
                            Debug.Log("input variation of " + inputVariation +" - note hit late");
                            AddScore(50);
                        }
                        else if (inputDownTime < hitObject.Time  && inputVariation > perfectMargin)
                        {
                            Debug.Log("input variation of " + inputVariation +" -note hit early");
                            AddScore(50);
                        }
                        else if (inputVariation <= perfectMargin)
                        {
                            Debug.Log("input variation of " + inputVariation +" -note hit perfectly");
                            AddScore(100);
                        }
                        else if (GameManager.Instance.accurateMusicTime > hitObject.Time)
                        {
                            Debug.Log("note missed!!");
                            SFXManager.Instance.PlaySound(2);
                        }
                    }
                }
            }
            else //only other note type would be hold note
            {
                if (Input.GetKeyDown(inputs[GetRealLaneNumber(hitObject.X)]))
                {
                    isHeld = true;
                    holdRoutine = StartCoroutine(HeldButtonTick());
                }

                if (Input.GetKeyUp(inputs[GetRealLaneNumber(hitObject.X)]))
                {
                    isHeld = false;
                    if (holdRoutine != null)
                    {
                        StopCoroutine(holdRoutine);
                    }
                }
            }   
        }
        
    }

    IEnumerator HeldButtonTick()
    {
        while (isHeld)
        {
            AddScore(10);
            yield return new WaitForSeconds((GameManager.Instance.BPM / 60f)/4);
        }
    }
    int GetRealLaneNumber(int osuNumber)
    {
        return Mathf.FloorToInt(osuNumber / 64f);
    }

    void AddScore(int i)
    {
        GameManager.Instance.AddScore(i);
    }
}
