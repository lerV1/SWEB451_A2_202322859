using UnityEngine;

public class HammerRotator : MonoBehaviour
{
    [SerializeField] private float rotationspeed = 90f;
    private void Update()
    {
        transform.Rotate(0f, rotationspeed * Time.deltaTime, 0f);
    }
}
