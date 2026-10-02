using UnityEngine;
using System;
public class ResetZone : MonoBehaviour
{
    public static event Action PlayerRespawned;
    private void OnTriggerEnter(Collider other)
    {
        PlayerMotor motor =
        other.GetComponentInParent<PlayerMotor>();
        if (motor != null)
        {
            motor.Respawn();
            if (PlayerRespawned != null) {
                PlayerRespawned.Invoke();
            }
        }
    }
}