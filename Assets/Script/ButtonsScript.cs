using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    public SceneController SceneController;
    [Header("======GameObjects to transitions======")]
    public GameObject gameMenu;
    public GameObject howToPlayMenu;
    public GameObject Objective;
    public GameObject gamePlay;
    public GameObject ZombieInfections;
    public GameObject Turrets;
    public Animator blackScreen;
    void Start()
    {
        SceneController = FindAnyObjectByType<SceneController>();
    }


    public void StartTheGame()
    {
        SceneController.NextScene("MainGame");
    }


    public void ExitTheGame()
    {
        Application.Quit();
    }

    public void HowToPlayMenu()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1f, gameMenu, howToPlayMenu));
    }
    public void ObjectiveMenu()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1f, howToPlayMenu, Objective));
    }
    public void GamePlayMenu()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1f, howToPlayMenu, gamePlay));
    }
    public void ZombieInfectionsMenu()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1f, howToPlayMenu, ZombieInfections));
    }

    public void TurretsMenu()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1f, howToPlayMenu, Turrets));
    }

    IEnumerator waitAFewSeconds(float secondsToWait, GameObject current, GameObject to)
    {
        yield return new WaitForSeconds(secondsToWait);
        current.SetActive(false);
        yield return new WaitForSeconds(secondsToWait);
        to.SetActive(true);
        blackScreen.SetTrigger("In");
    }
    //from back to ...
    public void Back()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1, howToPlayMenu, gameMenu));
    }
    public void Back1()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1, Objective, howToPlayMenu));
    }
    public void Back2()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1, gamePlay, howToPlayMenu));
    }
    public void Back3()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1, ZombieInfections, howToPlayMenu));
    }

    public void Back4()
    {
        blackScreen.SetTrigger("Out");
        StartCoroutine(waitAFewSeconds(1, Turrets, howToPlayMenu));
    }
    //=====
}
