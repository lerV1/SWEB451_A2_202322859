using UnityEngine;

// Makes the light on top of the signal tower blink.
public class SignalBeacon : MonoBehaviour
{
    [SerializeField] private Light beaconLight;
    [SerializeField] private float blinkSpeed = 1f;
    [SerializeField] private float maxIntensity = 3f;

    private void Update()
    {
        if (beaconLight == null)
        {
            return;
        }

        float blink = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        beaconLight.intensity = blink * maxIntensity;
    }
}
