using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float slowdownFactor = 0.05f;
    public float slowdownEndTransitionLength = 2f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void StopSlowMotion()
    {
        StartCoroutine(StopSlowMotionCoroutine());
        Time.timeScale = 1;
    }

    IEnumerator StopSlowMotionCoroutine()
    {
        while (1 - Time.timeScale > 0.01)
        {
            Time.timeScale += (1f / slowdownEndTransitionLength) * Time.unscaledDeltaTime;
            yield return null;
        }
        
    }
}