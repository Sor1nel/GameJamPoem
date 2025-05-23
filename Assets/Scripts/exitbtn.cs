using UnityEngine;
using UnityEngine.UI;

public class exitbtn : MonoBehaviour
{
    private int hidden = 0;
    public Button m_ExitButton;
    public Button m_LeaveButton;
    //public Button m_StayButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(m_ExitButton)
         m_ExitButton.onClick.AddListener(TaskOnClick);

        if(m_LeaveButton)
         m_LeaveButton.onClick.AddListener(StopPlay);

    }

    
    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        hidden^=1;
        Debug.Log(hidden);

            if(m_LeaveButton)
            {
                m_LeaveButton.gameObject.SetActive(hidden==1);
            } 
        
    }

    void StopPlay()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
