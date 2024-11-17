using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();


        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.PlaySound(collectedClip);
            controller.ChangeHealth(1);
            Destroy(gameObject);
            
        }
    }


}
