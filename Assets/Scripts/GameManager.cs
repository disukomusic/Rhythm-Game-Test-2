using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private OSUParser parser;

    public UnityEvent onSceneLoad;
    public UnityEvent countDownStart;
    public UnityEvent songStart;
    
    public float noteSpeed;
    public float circleRadius;
    public float notePreDelay;
    public bool isPlaying = false;

    public float accurateMusicTime;

    public float BPM;
    public float score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AccurateTimeManager accurateTimeManager;
    

    //public SpriteRenderer backgroundImage;
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple singleton game manager. bad. Is singleton. should only be one.");
            Destroy(gameObject);
        }
        Instance = this;
        score = 0;
    }

    private void Start()
    {
        notePreDelay = (circleRadius / noteSpeed) * 1000f;
        Debug.Log(notePreDelay);
        onSceneLoad.Invoke();

        BPM = parser.bpm;

    }
    
    public void AddScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }
    
    
    private void Update()
    {
        accurateMusicTime = accurateTimeManager.sampledTime;
        
        if (Input.GetKeyDown(KeyCode.Alpha6) && !isPlaying )
        {
            StartCoroutine(StartSongWithCountdown());
        }
    }


    private IEnumerator StartSongWithCountdown()
    {
        float _beat = 60f/ BPM;
        
        yield return new WaitForSeconds(_beat);
        SFXManager.Instance.PlaySound(1);
        countDownStart.Invoke();
        Debug.Log("3");
        yield return new WaitForSeconds(_beat);
        SFXManager.Instance.PlaySound(1);

        Debug.Log("2");
        yield return new WaitForSeconds(_beat);
        SFXManager.Instance.PlaySound(1);

        Debug.Log("1");
        yield return new WaitForSeconds(_beat);
        SFXManager.Instance.PlaySound(1);

        Debug.Log("Start!");
        
        BPM = parser.bpm;
        songStart.Invoke();
        isPlaying = true;
    }
}
