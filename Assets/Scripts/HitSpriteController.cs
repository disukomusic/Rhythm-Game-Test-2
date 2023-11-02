using System;
using System.Collections;
using UnityEngine;

public class HitSpriteController : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public float fadeDuration = 0.5f;
    public KeyCode keyCode;
    public GameObject hitSpriteObject; 

    private Sprite _initialSprite;
    private SpriteRenderer _hitSpriteRenderer;

    private NoteHit _desiredNote;

    void Awake() {
        _initialSprite = spriteRenderer.sprite;
        _hitSpriteRenderer = hitSpriteObject.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (Input.GetKeyDown(keyCode)) 
        {
            StartCoroutine(SwitchAndFade());
        }

        if (_desiredNote != null)
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (_desiredNote.canBePressed)
                {
                    GameManager.Instance.AddScore(1);
                    SFXManager.Instance.PlaySound(1);
                    _desiredNote.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            _desiredNote = other.GetComponent<NoteHit>();
        }
    }

    IEnumerator SwitchAndFade() {
        spriteRenderer.sprite = newSprite;

        float timer = 0f;
        Color hitSpriteColor = _hitSpriteRenderer.color;

        hitSpriteColor.a = 1f;
        _hitSpriteRenderer.color = hitSpriteColor;

        timer = 0f;

        while (timer < fadeDuration) {
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