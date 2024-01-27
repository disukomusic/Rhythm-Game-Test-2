using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public KeyCode keyCode;
    public MenuFace face;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MenuOption"))
        {
            other.GetComponent<MenuOption>().desiredKey = keyCode;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            face.targetPosition = transform.position;
        }
    }
}
