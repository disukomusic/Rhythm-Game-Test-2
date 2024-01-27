using System;
using UnityEngine;

//Adapted from Renaissance Coders
//https://github.com/coderDarren/RenaissanceCoders_UnityScripting

//Base class that can be used to sync anything to audio. 
public class AudioSyncer : MonoBehaviour
{
    //Determines what spectrum value will trigger a beat
    public float bias;
    
    //Minimum interval between beats
    public float timeStep;
    
    //How much time before the visualization completes
    public float timeToBeat;
    
    //How fast the object goes to rest after a beat
    public float restSmoothTime;

    //To determine if value went above or below bias during current frame
    private float m_previousAudioValue;
    private float m_audioValue;
    
    //Tracks timeStep interval
    private float m_timer;
    
    //keep track of whether or not sync object is in a beat state
    protected bool m_isBeat;
    
    private void Update()
    {
        //Child objects may want to have their own update code without overriding the base update code
        OnUpdate();
    }


    public virtual void OnUpdate()
    {
        //Store the previous and current audio spectrum values.
        m_previousAudioValue = m_audioValue;
        m_audioValue = AudioSpectrum.spectrumValue;

        //Check if the spectrum value went above or below the bias during the current frame
        //if so, check if enough time has passed since the last beat to trigger a new one
        if (m_previousAudioValue > bias && m_audioValue <= bias)
        {
            if (m_timer > timeStep)
            {
                OnBeat();
            }
        }

        if (m_previousAudioValue <= bias && m_audioValue > bias)
        {
            if (m_timer > timeStep)
            {
                OnBeat();
            }
        }

        //Increment the timer by the time since the last frame.
        m_timer += Time.deltaTime;
    }
    
    public virtual void OnBeat()
    {
        //Log the beat event.
        //Debug.Log("beat");
        
        //Reset the timer and set the beat state to true.
        m_timer = 0;
        m_isBeat = true;
    }
}