using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class AlertManager : MonoBehaviour
{
    public TMP_Text alertTextPrefab; // Reference to the TMP text prefab
    private TMP_Text currentAlertText; // Reference to the current alert text

    public static AlertManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //currentAlertText = Instantiate(alertTextPrefab, transform);
    }

    // Function to show an alert
    public void ShowAlert(string message)
    {
        // Set the message
        currentAlertText.text = message;

        // Start fading in
        StartCoroutine(FadeInAndOut());
    }

    // Coroutine to fade in and out the alert text
    private IEnumerator FadeInAndOut()
    {
        // Fade in
        currentAlertText.alpha = 0f;
        while (currentAlertText.alpha < 1f)
        {
            currentAlertText.alpha += Time.deltaTime * 3f;
            yield return null;
        }

        // Wait for a second
        yield return new WaitForSeconds(0.5f);

        // Fade out
        while (currentAlertText.alpha > 0f)
        {
            currentAlertText.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

    }
}
