using UnityEngine;
using System.Collections;

public class JumpscareManager : MonoBehaviour
{
    public InteractionType interaction; // Reference to the script that holds isWindowOpen
    public GameObject jumpscare;
    public AudioSource jumpscareAudio;

    public bool isMovingCamera = false;
    public bool isClosingWindow = false;

  

    private void Update()
    {
        if (isMovingCamera)
            MoveCameraWithBob();

        if (isClosingWindow)
            CloseWindowSmoothly();

        if (interaction != null && interaction.isWindowOpen)
        {
            StartCoroutine(JumpScare());
            
        }
    }

    IEnumerator JumpScare()
    {
        yield return new WaitForSeconds(15f);
        jumpscare.SetActive(true);

        if (jumpscareAudio != null && jumpscareAudio.clip != null)
        {
            jumpscareAudio.Play();
            yield return new WaitForSeconds(jumpscareAudio.clip.length);
        }
        else
        {
            Debug.LogWarning("Jumpscare audio or clip is not assigned.");
            yield return new WaitForSeconds(2f);
        }

        jumpscare.SetActive(false);
        //Application.Quit();
    }

    // Placeholder methods – implement your own logic
    private void MoveCameraWithBob()
    {
        // Camera bobbing logic here
    }

    private void CloseWindowSmoothly()
    {
        // Smooth window closing logic here
    }
}
