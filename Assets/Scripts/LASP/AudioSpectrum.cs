using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from Renaissance Coders
//https://github.com/coderDarren/RenaissanceCoders_UnityScripting

//Captures and stores the audio spectrum data from currently playing audio in the scene
public class AudioSpectrum : MonoBehaviour
{
    //Array to hold spectrum data
    private float[] m_audioSpectrum; 
    
    //Public property to access the spectrum value from other scripts
    public static float spectrumValue { get; private set; } 
    
    private void Start()
    {
        //Initialize the spectrum array with a size of 128: a power of 2 is required by the GetSpectrumData() method
        m_audioSpectrum = new float[128];
    }

    private void Update()
    {
        //Populate spectrum array with values from currently playing audio
        AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);

        // If the m_audioSpectrum array has data, update the spectrumValue property with the first element
        //Multiplied by 100 to make it easier to work with in other scripts. (arbitrary value)
        if (m_audioSpectrum != null && m_audioSpectrum.Length > 0)
        {
            spectrumValue = m_audioSpectrum[0] * 100;
        }
    }
}
