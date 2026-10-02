using UnityEngine;

public class CourseTimer : MonoBehaviour
{
    public float ElapsedTime { get; private set; }
    private bool timerRunning = true;
    private void OnEnable()
    {
        GoalZone.CourseCompleted += StopTimer;
    }
    private void OnDisable()
    {
        GoalZone.CourseCompleted -= StopTimer;
    }
    private void Update()
    {
        if (timerRunning)
        {
            ElapsedTime += Time.deltaTime;
        }
    }
    private void StopTimer()
    {
        timerRunning = false;

        Debug.Log(
            "Final Time: " +
            ElapsedTime.ToString("F2") +
            " seconds");
    }
}