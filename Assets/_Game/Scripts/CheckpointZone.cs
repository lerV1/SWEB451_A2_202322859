using UnityEngine;

public class CheckpointZone : MonoBehaviour
{
    [SerializeField] private Transform respawnMarker;
    private bool activated;
    private void OnTriggerEnter(Collider other)
    {
        if (activated)
        {
            return;
        }
        PlayerMotor motor =
            other.GetComponentInParent<PlayerMotor>();

        if (motor == null)
        {
            return;
        }
        motor.SetRespawnPosition(respawnMarker.position);
        activated = true;
        Debug.Log("Checkpoint activated.");
    }
}