using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.OSUImporter;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

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
    public float offset;
    public bool isPlaying = false;

    public float accurateMusicTime => accurateTimeManager.sampledTime;
    public int msAccurateMusicTime => accurateTimeManager.msSampleTime;

    public float BPM;
    public float score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AccurateTimeManager accurateTimeManager;
    private Dictionary<int, List<ActiveHitObject>> hitsByLaneMap;

    [SerializeField] private HitSpriteController[] hitSpriteControllers;
    //public SpriteRenderer backgroundImage;
    
    void Awake()
    {
        hitsByLaneMap = new Dictionary<int, List<ActiveHitObject>>();
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
        hitSpriteControllers = GameObject.FindObjectsOfType<HitSpriteController>();
        
        notePreDelay = ((circleRadius / noteSpeed) * 1000f) + offset;
        Debug.Log(notePreDelay);
        onSceneLoad.Invoke();

        BPM = parser.bpm;
        
    }
    
    public void UpdateScore()
    {
        score = 0;
        foreach (var lane in hitSpriteControllers)
        {
            score += lane.Score;
        }
        scoreText.text = score.ToString();
    }
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && !isPlaying )
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

    public List<ActiveHitObject> GetLaneHitObjects(int lane)
    {
        //lazy cache
        if (hitsByLaneMap.ContainsKey(lane))
        {
            return hitsByLaneMap[lane];
        }
        else
        {
            var hitObjectsForLane = parser.activeHitObjects.Where(x=>lane == ScoreManager.GetRealLaneNumber(x.hitObject.X)).ToList();
            hitsByLaneMap[lane] = hitObjectsForLane;
            return hitObjectsForLane;
        }
    }

    public void OnSongEnd()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
