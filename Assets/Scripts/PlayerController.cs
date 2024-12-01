using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    //Score 
    public int score;

    // game over
    public GameObject gameOverText;
    bool gameOver;

    // win or lose sounds ------------ audio mod
    public AudioClip win;
    bool winEnd;
    public AudioClip lose;
    bool loseEnd;

    // Variables related to audio
    AudioSource audioSource;

    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;


    // Variables related to the health system
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    public GameObject healthEffectPrefab;
    public GameObject hitEffectPrefab;


    // Variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;


    // Variables related to animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);
    public InputAction talkAction;


    // Variables related to projectiles
    public GameObject projectilePrefab;
    public InputAction launchAction;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        talkAction.Enable();
        talkAction.performed += FindFriend;

        launchAction.Enable();
        //launchAction.performed += Launch; <---causes compiler error CS0123, game functions fine without it

        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        gameOver = false;

        // set win and lose to false  --------------- audio mod
        loseEnd = false;
        winEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();


        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }


        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        // Detect input for projectile launch
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // Detect input for NPC interaction
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            }
        }

        // ifs to display game over screen
        if (currentHealth <= 0)
        {
            gameOverText.SetActive(true);
            gameOver = true;
            speed = 0;

            // set lose to true after health is <0 -------------- audio mod
            loseEnd = true;
        }

        if (score >= 2)
        {
            gameOverText.SetActive(true);
            gameOver = true;
            speed = 0;

            // set win to true after score is >2 --------------- audio mod
            winEnd = true;
        }

        // if win or lose is true, then play corresponding sound --------------- audio mod
        if (winEnd == true)
        {
            audioSource.PlayOneShot(win);
        }
        if (loseEnd == true)
        {
            audioSource.PlayOneShot(lose);
        }

        if (Input.GetKey(KeyCode.R)) // check to see if the player is pressing R

        {
            if (gameOver == true) // check to see if the game over boolean has been set to true

            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene, which results in a restart of whatever scene the player is currently in
            }
        }
    }


    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);

    }


    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");

            GameObject hitEffectObject = Instantiate(hitEffectPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        if (amount>0)
        {
            GameObject healthEffectObject = Instantiate(healthEffectPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        }


        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);


        animator.SetTrigger("Launch");
    }
    void FindFriend(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (hit.collider != null)
            {
                UIHandler.instance.DisplayDialogue();
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);

    }

    public void ChangeScore(int scoreAmount)
    {
        score += scoreAmount; //increase score amount
    }


}

