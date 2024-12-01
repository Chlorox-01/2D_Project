using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

// set up the same as HealthCollectible but sets the player speed to 6 insetad of increasing health by +1 ---------- gameplay mod

public class SpeedCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();


        if (controller != null && controller.speed <6)
        {
            controller.PlaySound(collectedClip);
            controller.speed = 6;
            Destroy(gameObject);

        }

    }
}
