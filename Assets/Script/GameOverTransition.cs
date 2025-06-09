using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTransition : MonoBehaviour
{
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
