using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }

        Rigidbody body = collision.rigidbody;

        if (body == null)
        {
            return;
        }

        Vector3 velocity = body.linearVelocity;
        velocity.y = launchSpeed;
        body.linearVelocity = velocity;
    }
}
