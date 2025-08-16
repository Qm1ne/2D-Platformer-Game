using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour //shouldv made it a singleton
{
    [SerializeField] private Animator player_animator;
    [SerializeField] private Rigidbody2D player_rigidbody;
    [SerializeField] private Collider2D player_collider;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool havekey = false;

    [SerializeField] private string playerName = "Player";
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool Grounded = true;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool haveWeapon = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isCrouching = false;
    [SerializeField] private float attackpower = 10f;
    [SerializeField] private float attackRange = 1.5f;
    public void Generaterandomnumber()
    {
        // Example of generating a random number
        int randomNumber = Random.Range(1, 100);
    }
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }
    public bool HaveKey
    {
        get { return havekey; }
        set { havekey = value; }
    }
    public float Health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if (health <= 0)
            {
                IsDead = true; // Automatically set player to dead if health reaches 0
            }
        }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set
        {
            isDead = value;
            if (isDead)
            {
                canMove = false;
                canJump = false;
                canAttack = false;
                canInteract = false;
                // Additional logic for when the player dies, e.g., playing death animation
                Debug.Log("Player is dead.");
                player_animator.SetTrigger("Hurt");
                player_animator.SetTrigger("Dead");
                

                
            }
            else
            {
                canMove = true;
                canJump = true;
                canAttack = true;
                canInteract = true;
                // Additional logic for when the player respawns or is revived
                Debug.Log("Player is alive.");
                player_animator.ResetTrigger("Dead");
                player_animator.SetTrigger("Respawn");
            }
        }
    }
    public bool IsCrouching
    {
        get { return isCrouching; }
        set
        {
            isCrouching = value;
            if (isCrouching)
            {
                // Logic for crouching, e.g., reduce speed or change collider size
                Debug.Log("Player is crouching.");
            }
            else
            {
                // Logic for standing up, e.g., restore speed or collider size
                Debug.Log("Player is standing up.");
            }
        }
    }
    public float AttackPower
    {
        get { return attackpower; }
        set
        {
            if (haveWeapon) // Ensure the player has a weapon before setting attack power
            {
                attackpower = value + 5f; // Example of modifying attack power
            }
            else
            {
                attackpower = value; // Set attack power without modification if no weapon
            }

        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        // Initialize any necessary components or variables here
        player_rigidbody = GetComponent<Rigidbody2D>();
        player_collider = GetComponent<Collider2D>();
        player_animator = GetComponent<Animator>();
    }
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    void Start()
    {
        // Store the original collider size and offset
        if (player_collider is BoxCollider2D boxCollider)
        {
            originalColliderSize = boxCollider.size;
            originalColliderOffset = boxCollider.offset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleInteraction();
    }
    private void HandleMovement()
    {
        if (!canMove) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0, 0) * speed * Time.deltaTime;
        transform.Translate(movement);

        // Handle sprite flipping and animation
        if (moveHorizontal != 0)
        {
            // Flip the sprite based on movement direction
            transform.localScale = new Vector3(Mathf.Sign(moveHorizontal), 1, 1);
            // Set the exact horizontal speed for the blend tree
            player_animator.SetFloat("HorizontalSpeed", Mathf.Abs(moveHorizontal));
        }
        else
        {
            player_animator.SetFloat("HorizontalSpeed", 0);
        }
    }
    private void HandleJump()
    {
        float moveVertical = Input.GetAxis("Vertical");
        bool isInAir = player_rigidbody.velocity.y != 0;

        // Only proceed if we have a BoxCollider2D
        if (player_collider is BoxCollider2D boxCollider)
        {
            if (canJump && moveVertical > 0 && !isInAir)
            {
                // Jumping - make collider taller and thinner
                player_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                player_animator.SetFloat("VerticalSpeed", moveVertical);
                player_animator.SetBool("Grounded", false);

                // Jumping - make collider taller and thinner, move it up
                boxCollider.size = new Vector2(originalColliderSize.x * 0.8f, originalColliderSize.y * 0.5f);
                // Move the collider up by 10% of its height to match the jump animation
                boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y + originalColliderSize.y * 0.1f);
            }
            else if (moveVertical < 0)
            {
                // Crouching - make collider shorter and wider, move it down
                IsCrouching = true;
                player_animator.SetBool("Crouching", true);

                boxCollider.size = new Vector2(originalColliderSize.x * 1.2f, originalColliderSize.y * 0.5f);
                // Move the collider down by 25% of its original height to match the crouch
                boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - originalColliderSize.y * 0.25f);
            }
            else if (moveVertical == 0 && !isInAir)
            {
                // Normal state - restore original collider size and position
                IsCrouching = false;
                player_animator.SetBool("Grounded", true);
                player_animator.SetBool("Crouching", false);

                boxCollider.size = originalColliderSize;
                boxCollider.offset = originalColliderOffset;
            }
        }
    }
    private void HandleAttack()
    {
        if (!canAttack || !Input.GetButtonDown("Fire1") || !haveWeapon) return;

        // Implement attack logic here
        Debug.Log("Player attacked!");
    }

    private void HandleInteraction()
    {
        if (!canInteract || !Input.GetButtonDown("Interact")) return;

        // Implement interaction logic here
        Debug.Log("Player interacted with an object!");
    }
    
}
