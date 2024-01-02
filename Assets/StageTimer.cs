using UnityEngine;

public class StageTimer : MonoBehaviour
{
    private bool carInsideTrigger = false;
    [SerializeField]
    private float timer = 0f;
    public float prevTime = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("car"))
        {
            if (!carInsideTrigger)
            {
                carInsideTrigger = true;
                prevTime = timer;
                timer = 0f; // Reset the timer when the car enters the trigger
                Debug.Log("Car entered the trigger. Timer started.");
            }
            else
            {
                carInsideTrigger = false;
                Debug.Log("Car entered the trigger again. Timer stopped. Total time: " + timer.ToString("F2") + " seconds.");
            }
        }
    }

    void Update()
    {
        if (carInsideTrigger)
        {
            timer += Time.deltaTime;
        }
    }
}

