using System.Collections;
using UnityEngine;

public class SpeedX2 : MonoBehaviour
{
    public Animator CanavasAnimatorEsc;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                CanavasAnimatorEsc.SetTrigger("EscapeIn");
            }
            else
            {
                ResumeGame();
                Time.timeScale = 1.0f;
            }
            return;
        }

        if (isPaused) return;

        Time.timeScale = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? 2.0f : 1.0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        CanavasAnimatorEsc.SetTrigger("EscapeOut");
    }

    public void Retry()
    {
        SceneController sceneController = FindAnyObjectByType<SceneController>();
        sceneController.NextScene("MainGame");
    }

    public void MainMenuMenu()
    {
        SceneController sceneController = FindAnyObjectByType<SceneController>();
        sceneController.NextScene("GameMenu");
    }
}
