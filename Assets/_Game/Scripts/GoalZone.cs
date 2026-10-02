using UnityEngine;
using System;
[RequireComponent(typeof(Collider))]
public class GoalZone : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private Renderer indicatorRenderer;
    [SerializeField] private Light goalLight;
    [SerializeField] private AudioSource unlockAudio;
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;
    [SerializeField] private float lockedIntensity = 0.5f;
    [SerializeField] private float unlockedIntensity = 3f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.4f;
    private bool unlocked;
    private bool completionReported;
    private Material runtimeMaterial;
    public static event Action CourseCompleted;
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        if (indicatorRenderer != null)
        {
            runtimeMaterial = indicatorRenderer.material;
        }
        ApplyFeedback();
    }
    private void OnEnable()
    {
        if (session != null)
        {
            session.GoalBecameAvailable += Unlock;
        }
    }
    private void OnDisable()
    {
        if (session != null)
        {
            session.GoalBecameAvailable -= Unlock;
        }
    }
    private void Unlock()
    {
        unlocked = true;
        ApplyFeedback();
        if (unlockAudio != null)
        {
            unlockAudio.Play();
        }
        Debug.Log("Goal unlocked.");
    }
    private void ApplyFeedback()
    {
        if (runtimeMaterial != null)
        {
            runtimeMaterial.color =
            unlocked ? unlockedColor : lockedColor;
        }
        if (goalLight != null)
        {
            goalLight.intensity = unlocked
            ? unlockedIntensity
            : lockedIntensity;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (unlocked)
        {
            Debug.Log("Course complete! Respawns: " +
                session.RespawnCount);

            if (!completionReported)
            {
                completionReported = true;
                if (CourseCompleted != null)
                {
                    CourseCompleted.Invoke();
                }
            }
        }
        else
        {
            Debug.Log(
            "Goal locked. Remaining collectibles: " +
            session.RemainingCollectibles);
        }


    }
    private void Update()
    {
        if (!unlocked && goalLight != null)
        {
            float pulse =
                Mathf.PingPong(Time.time * pulseSpeed, pulseAmount);

            goalLight.intensity =
                lockedIntensity + pulse;
        }
    }
}