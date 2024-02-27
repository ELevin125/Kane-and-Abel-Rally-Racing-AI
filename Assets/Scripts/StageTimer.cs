using System;
using TMPro;
using UnityEngine;

public class StageTimer : MonoBehaviour
{
    private bool carInsideTrigger = false;
    [SerializeField]
    private float timer = 0f;
    public float prevTime = 0f;

    public TMP_Text clockText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("car"))
        {
            if (!carInsideTrigger)
            {
                carInsideTrigger = true;
                prevTime = timer;
                timer = 0f; // Reset the timer when the car enters the trigger
                Debug.Log("Timer started.");
            }
            else
            {
                carInsideTrigger = false;
                Debug.Log("Timer stopped. Total time: " + timer.ToString("F2") + " seconds.");
            }
        }
    }

    void Update()
    {
        if (carInsideTrigger)
        {
            timer += Time.deltaTime;
            clockText.text = FormatTime(timer); 
        }

    }

    // Format time in "00:00" format
    string FormatTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:00}:{1:00}:{2:000}", time.Minutes, time.Seconds, time.Milliseconds);
    }
}

