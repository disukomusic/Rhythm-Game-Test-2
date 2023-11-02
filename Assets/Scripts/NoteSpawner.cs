using System;
using System.Collections;
using System.Collections.Generic;
using HDyar.OSUImporter;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance;
    
    public List<GameObject> notePool;
    public GameObject notePrefab;
    public int startingPoolAmt;

    public enum NoteType
    {
        Normal,
        Hold,
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        notePool = new List<GameObject>();
        GameObject tmp;

        for (int i = 0; i < startingPoolAmt; i++)
        {
            tmp = Instantiate(notePrefab);
            tmp.SetActive(false);
            notePool.Add(tmp);
        }
    }

    public void SpawnNote(int lane, NoteType type, int noteStartTime, int holdNoteEndTime)
    {
        
        GameObject note = GetPooledObject();
        if (note != null)
        {
            note.SetActive(true);
            Note desiredNote = note.GetComponent<Note>();
            desiredNote.isHoldNote = false;
            desiredNote.targetLane = lane;
            desiredNote.speed = GameManager.Instance.noteSpeed;
            note.transform.position = transform.position;
            note.transform.rotation = Quaternion.identity;
            desiredNote.noteStartTime = noteStartTime;
            
            if (type == NoteType.Hold)
            {
                desiredNote.isHoldNote = true;
                desiredNote.holdNoteEndTime = holdNoteEndTime;
            }
            
            desiredNote.OnNoteSpawn();
            
            Debug.Log("spawned note at lane " + lane + " of type " + type + " with start time " + noteStartTime +
                      " and end time " + holdNoteEndTime);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < startingPoolAmt; i++)
        {
            if (!notePool[i].activeInHierarchy)
            {
                return notePool[i];
            }
        }

        return null;
    }
}
