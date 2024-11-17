using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public class EnemyController : MonoBehaviour
{
    private PlayerController playerController; // this line of code creates a variable called "playerController" to store information about the PlayerController script!

    // Public variables
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;

    // Private variables
    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;
    bool aggressive = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;

        GameObject playerControllerObject = GameObject.FindWithTag("PlayerController"); //this line of code finds the PlayerController script by looking for a "PlayerController" tag on Ruby

        if (playerControllerObject != null)

        {

            playerController = playerControllerObject.GetComponent<PlayerController>(); //and this line of code finds the playerController and then stores it in a variable

            print("Found the PlayerConroller Script!");

        }

        if (playerController == null)

        {

            print("Cannot find GameController Script!");

        }


    }

    // Update is called every frame
    void Update()
    {
        if (!aggressive)
        {
            return;
        }

    }


    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        if (!aggressive)
        {
            return;
        }

        timer -= Time.deltaTime;


        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }


        rigidbody2d.MovePosition(position);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);

        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();


        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        aggressive = false;
        GetComponent<Rigidbody2D>().simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();


        if (playerController != null)
        {
            
            playerController.ChangeScore(1); //this line of code is increasing Ruby's score by 1!
           
        }
    }

}
