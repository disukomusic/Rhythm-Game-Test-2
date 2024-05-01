using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Blooper.TransitionEffects;


public class MenuOption : MonoBehaviour
{
    public string scene;
    public KeyCode desiredKey;

    public GameObject confirmSprite;
    
    public static MenuOption CurrentlySelectedMenuObject;

    private void Start()
    {
        confirmSprite.SetActive(false);
    }

    private void Update()
    {
        confirmSprite.SetActive(CurrentlySelectedMenuObject == this);
    }

    public void OnSelect()
    {
        if (CurrentlySelectedMenuObject == this)
        {
            if (scene.Length > 0)
            {
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log("No Scene for the selected menu option!");
                AlertManager.Instance.ShowAlert("Not Implemented (yet)");
            }
        }
        else
        {
            CurrentlySelectedMenuObject = this;
        }
    }
}
