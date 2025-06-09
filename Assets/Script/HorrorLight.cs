using System.Collections;
using TMPro;
using UnityEngine;

public class HorrorLight : MonoBehaviour
{
    public Light[] spotLights;
    public float blackoutInterval = 10f;
    public float blackoutDuration = 2f;
    public bool enableFullBlackout = true; // Toggle full blackout
    public Spawner spawner;

    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
        StartCoroutine(BlackoutEffect());
    }
    bool once = false;
    void Update()
    {
        if (spawner.rounds == 11 && !once)
        {
            StartCoroutine(bosslights());
            once = true;
        }
    }

    IEnumerator bosslights()
    {
        foreach (Light light in spotLights)
        {
            if (light.enabled == true)
            {
                light.enabled = false;
            }
            light.color = Color.red;
        }
        yield return new WaitForSeconds(1.5f);
        foreach (Light light in spotLights)
        {
            if (light.enabled == false)
            {
                light.enabled = true;
            }
            light.color = Color.red;
        }
    }

    IEnumerator BlackoutEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(blackoutInterval);

            // Optional full blackout effect
            if (enableFullBlackout)
            {
                foreach (Light light in spotLights)
                    light.enabled = false;

                yield return new WaitForSeconds(blackoutDuration);

                foreach (Light light in spotLights)
                    light.enabled = true;
            }

            // Random flickering effect
            int randomLight = Random.Range(0, spotLights.Length);

            for (int i = 0; i < Random.Range(3, 6); i++) // Random number of flickers
            {
                spotLights[randomLight].enabled = false;
                yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
                spotLights[randomLight].enabled = true;
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            }
        }
    }
}
