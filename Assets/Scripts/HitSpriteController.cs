using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HitSpriteController : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public float fadeDuration = 0.5f;
    public KeyCode keyCode;
    public GameObject hitSpriteObject; 

    private Sprite _initialSprite;
    private SpriteRenderer _hitSpriteRenderer;

    private NoteHit _desiredNote;
    private Note _desiredNoteObject;

    private PlayerHitData _currentlyPressingHit;
    public List<PlayerHitData> _lanePresses;

    public float Score => score;
    private float score;
    //todo: not manually write this.
    public int Lane;
    void Awake()
    {
        _lanePresses = new List<PlayerHitData>();
        _initialSprite = spriteRenderer.sprite;
        _hitSpriteRenderer = hitSpriteObject.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (Input.GetKeyDown(keyCode)) 
        {
            StartCoroutine(SwitchAndFade());
        }

        if (Input.GetKeyDown(keyCode))
        {
            // if (_desiredNote.canBePressed)
            // {
            //     //GameManager.Instance.AddScore(100f);
            //     SFXManager.Instance.PlaySound(1);
            //     _desiredNote.gameObject.SetActive(false);
            // }

            _currentlyPressingHit = new PlayerHitData();
            _currentlyPressingHit.pressTime = GameManager.Instance.msAccurateMusicTime;
        }
        if (Input.GetKeyUp(keyCode))
        {
            // if (_desiredNote.canBePressed)
            // {
            //     //GameManager.Instance.AddScore(100f);
            //     SFXManager.Instance.PlaySound(1);
            //     _desiredNote.gameObject.SetActive(false);
            // }
            _currentlyPressingHit.releaseTime = GameManager.Instance.msAccurateMusicTime;
            _lanePresses.Add(_currentlyPressingHit);
            _currentlyPressingHit = null;
            score = RecalculateLaneScore();
            GameManager.Instance.UpdateScore();
        }
        
    }

    private float RecalculateLaneScore()
    {
        float laneScore = 0;
        var hits = GameManager.Instance.GetLaneHitObjects(Lane);
        foreach (var press in (_lanePresses))
        {
            //we already figured this one out
            if (press.hitObject != null)
            {
                laneScore += press.GetScoreForPress();
                //SFXManager.Instance.PlaySound(1);
                //AlertManager.Instance.ShowAlert("Nice!");
                continue;
            }

            //we have not let go yet, also we must be caught up with the present so lets just end now. No score till you let go.
            if (press.releaseTime < press.pressTime)
            {
                return laneScore;
            }
            
            //search for hit from hits.
            foreach (var hit in hits)
            {
                if (IsValidHitForPress(hit, press))
                {
                    //SFXManager.Instance.PlaySound(1);
                    //AlertManager.Instance.ShowAlert("Nice!");

                    hit.press = press;
                    press.hitObject = hit;
                    laneScore += press.GetScoreForPress();
                    continue;
                }
            }

            if (press.hitObject == null)
            {
                //no valid hit. :(

                //laneScore -= 500;
            }
        }

        foreach (var noPressHits in hits.Where(x=>x.hitObject == null))
        {
            Debug.Log("miss");

            //laneScore -= 1000;
        }
        //Debug.Log(gameObject.name + " has a score of" + laneScore);

        return laneScore;
    }

    private bool IsValidHitForPress(ActiveHitObject hit, PlayerHitData press)
    {
        int threshold = 1000;
        int delta = Mathf.Abs(press.pressTime - hit.Time);
        //todo: if it's a bad lane
        //todo make this static 
        
        
        // if (press.pressTime > hit.Time && press.pressTime < hit.hitObject.Time) //normal notes have an end time of -1 so press time will ALWAYS be greater than 
        // {
        //     return true;
        // }

        if (Mathf.Abs(press.pressTime - hit.Time) < threshold)
        {
            Debug.Log("note hit, off by" + delta);
            return true;
        }
        
        if (delta > threshold)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Note"))
    //     {
    //         _desiredNote = other.GetComponent<NoteHit>();
    //        _desiredNoteObject = other.GetComponent<Note>();
    //     }
    // }

    IEnumerator SwitchAndFade() 
    {   
        spriteRenderer.sprite = newSprite;

        float timer = 0f;
        Color hitSpriteColor = _hitSpriteRenderer.color;

        hitSpriteColor.a = 1f;
        _hitSpriteRenderer.color = hitSpriteColor;

        timer = 0f;

        while (timer < fadeDuration) 
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            hitSpriteColor.a = alpha;
            _hitSpriteRenderer.color = hitSpriteColor;

            timer += Time.deltaTime;
            yield return null;
        }

        hitSpriteColor.a = 0f;
        _hitSpriteRenderer.color = hitSpriteColor;

        spriteRenderer.sprite = _initialSprite;
    }
}