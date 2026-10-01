using UnityEngine;

public class RotatingBar : MonoBehaviour
{
    [SerializeField] private float rotationspeed = 90f;

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationspeed * Time.deltaTime);
    }

}
