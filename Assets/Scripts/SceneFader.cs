using Blooper.TransitionEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOnStart : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Transition.TransitionInToScene(TransitionType.CircleWipe,0.1f, 0.85f, Color.white));
    }
}