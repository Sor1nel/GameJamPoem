using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    
    public Button m_StartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if(m_StartButton)
         m_StartButton.onClick.AddListener(StartPL);
    }

    void StartPL()
    {
        SceneManager.LoadScene(2);
    }
}
