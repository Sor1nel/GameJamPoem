using UnityEngine;
using UnityEngine.UI;

public class MaxSoundMgr : MonoBehaviour
{
    public bool playing = true;
    public Text m_timer_text;
    public float m_seconds_left = 180f;

    public bool[] m_riddles_ = {};


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        DisplayTime(m_seconds_left);
        m_seconds_left-=0.02f;
    }

    void DisplayTime(float timeToDisplay)
    {
        if(m_timer_text)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            m_timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
