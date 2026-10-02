
using System;
using UnityEngine;
public class GameSession : MonoBehaviour
{
    [SerializeField] private GameConfig config;
    public int CollectedCount { get; private set; }
    public bool IsGoalUnlocked { get; private set; }
    public int RespawnCount { get; private set; }
    public event Action GoalBecameAvailable;
    private void Awake()
    {
        CollectedCount = 0;
        IsGoalUnlocked = false;
        RespawnCount = 0;
    }
    private void HandlePlayerRespawned()
    {
        RespawnCount++;
        Debug.Log("Respawns: " + RespawnCount);
    }
    private void OnEnable()
    {
        CollectiblePickup.Collected += HandleCollected;
        ResetZone.PlayerRespawned += HandlePlayerRespawned;
    }
    private void OnDisable()
    {
        CollectiblePickup.Collected -= HandleCollected;
        ResetZone.PlayerRespawned -= HandlePlayerRespawned;
    }

    private void HandleCollected(int amount)
    {
        CollectedCount += amount;
        Debug.Log(
        $"Energy {CollectedCount}/" +
        $"{config.requiredCollectibles}");
        if (!IsGoalUnlocked &&
        CollectedCount >= config.requiredCollectibles)
        {
            IsGoalUnlocked = true;
            Debug.Log("Goal available.");
            GoalBecameAvailable?.Invoke();
        }
    }
    public int RemainingCollectibles
    {
        get
        {
            int remaining = config.requiredCollectibles - CollectedCount;
            return Mathf.Max(remaining, 0);
        }
    }
}
