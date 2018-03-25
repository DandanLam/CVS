using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicMixer : MonoBehaviour {

    public AudioMixerSnapshot outOfCombat;
    public AudioMixerSnapshot inCombat;
    public float bpm = 128;

    private float m_TransitionIn;
    private float m_TransitionOut;
    private float m_QuarterNote;


    void Start () {
        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote;
        m_TransitionOut = m_QuarterNote * 32;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.enemyProximityTag))
        {
            inCombat.TransitionTo(m_TransitionIn);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(StringConstants.enemyProximityTag))
        {
            outOfCombat.TransitionTo(m_TransitionOut);
        }
    }

}
