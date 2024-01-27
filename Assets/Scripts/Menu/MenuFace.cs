using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFace : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 initPos;
    public GameObject targetObject;
    
    private int previousIndex = -1;
    public SpriteRenderer faceSprite;
    public Sprite[] faceImages;
    
    public float scaleIncreaseAmount = 0.01f;
    public float animationDuration = 0.1f;
    public float bounceAmount = 0.01f;
    
    void Start()
    {
        initPos = transform.position;
        targetPosition = initPos;
    }
    
    float smoothTime = 0.3f; 
    Vector3 velocity = Vector3.zero;
    void Update()
    {
        float _lerpFactor = 0.1f;
        Vector3 target = Vector3.Lerp(initPos, targetPosition, _lerpFactor);

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);

        if (Input.anyKeyDown)
        {
            Vector2 _direction = targetPosition - initPos;
            RaycastHit2D hit = Physics2D.Raycast(initPos, _direction);
            Debug.DrawLine(initPos, targetPosition, Color.red);
            
            ChangeFace();
            
            StartCoroutine(Pulse());
            
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("MenuOption"))
                {
                    Debug.Log("selected option" + hit.collider.name);
                    hit.collider.GetComponent<MenuOption>().OnSelect();
                    targetObject = hit.collider.gameObject;
                }
            }
        }
        
        if (targetObject != null)
        {
            targetPosition = targetObject.transform.position;
        }
    }
    
    void ChangeFace()
    {
        int newIndex = Random.Range(0, faceImages.Length);

        while (newIndex == previousIndex)
        {
            newIndex = Random.Range(0, faceImages.Length);
        }

        faceSprite.sprite = faceImages[newIndex];
        previousIndex = newIndex;
    }
    

    IEnumerator Pulse()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale + new Vector3(scaleIncreaseAmount, scaleIncreaseAmount, scaleIncreaseAmount);

        float timer = 0f;

        while (timer < animationDuration)
        {
            float scale = Mathf.Lerp(0f, 1f, timer / animationDuration);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, scale);

            timer += Time.deltaTime;
            yield return null;
        }

        float bounceTimer = 0f;
        while (bounceTimer < 0.2f)
        {
            float bounceScale = Mathf.Sin(bounceTimer * Mathf.PI * 5) * bounceAmount;
            transform.localScale = targetScale + new Vector3(bounceScale, bounceScale, bounceScale);

            bounceTimer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}    