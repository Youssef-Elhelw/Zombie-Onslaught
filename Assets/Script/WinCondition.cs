using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public Spawner spawner;
    public Animator CanavasWonAnimator;
    public Animator DayLight;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
    }

    // Update is called once per frame
    bool justOneTime = false;
    void Update()
    {
        if (spawner.won && !justOneTime)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Victory, 0.6f);
            CanavasWonAnimator.SetTrigger("EscapeIn");
            DayLight.SetTrigger("Won");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.applaus, 0.4f);
            justOneTime = true;
        }
    }
}
