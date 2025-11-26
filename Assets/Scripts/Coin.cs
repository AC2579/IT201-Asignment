using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioSource pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the sound
            pickupSound.Play();
        }
    }
}
