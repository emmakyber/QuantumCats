using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCats : MonoBehaviour
{
    private AudioSource audioSource;
    public float framesPerSecond, frameDuration, distanceFromGround, jumpForce, raycastDif;
    public float moveSpeed = 10f;
    public float fallForce = 10f;
    public float raycastDistance = 1.5f;
    public Sprite[] sideSprites, endingSprites;
    public LayerMask groundLayer;
    public Vector2 targetPosition = new Vector2(-10, 20);
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Coroutine currentAnimation;
    private Vector2 endMove;
    public GameObject box;
    private Vector2 movement;
    private bool isMoving = false;
    private bool endReached = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsAtTarget())
        {
            MoveTowardsTarget();
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * fallForce * 2f);
        }
        else if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector2.down * fallForce, ForceMode2D.Impulse);
        }
    }

    private bool IsAtTarget()
    {
        return Vector2.Distance(transform.position, targetPosition) < 0.5f; // Use a small threshold to avoid precision issues
    }

    private void MoveTowardsTarget()
    {
        Vector2 directionToTarget = targetPosition - (Vector2)transform.position;
        float horizontalDirection = Mathf.Sign(directionToTarget.x);

        // Update movement
        rb.velocity = new Vector2(horizontalDirection * moveSpeed, rb.velocity.y);

        // Check if the sprite should jump
        if ((rb.velocity.x == 0 && IsGrounded()) || ShouldJump())
        {
            Jump();
        }

       // Handle movement animations and direction
        if (!isMoving && !endReached)
        {
            isMoving = true;
            UpdateMovementAnimation(true);
        }
        else if (isMoving && rb.velocity.x == 0 || endReached)
        {
            UpdateMovementAnimation(false);
        }

        // Flip sprite based on moving direction
        spriteRenderer.flipX = horizontalDirection < 0;
    }

    private bool ShouldJump()
    {

        // Horizontal raycast to check for obstacles in front of the sprite
        Vector2 forwardDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y + 0.5f); // Adjust vertical offset as needed

        RaycastHit2D forwardHit = Physics2D.Raycast(raycastOrigin, forwardDirection, raycastDif, groundLayer); // Increase distance as necessary

        // Vertical raycast to check for overhead obstacles
        Vector2 upwardDirection = Vector2.up;
        RaycastHit2D upwardHit = Physics2D.Raycast(raycastOrigin, upwardDirection, raycastDif, groundLayer); // Adjust vertical distance as necessary

        // Return true if there's a forward obstacle and no overhead obstacle
        return forwardHit.collider != null && !upwardHit.collider;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround, groundLayer);
        return hit.collider != null;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is tagged as "Box"
        if (collision.gameObject.CompareTag("Box"))
        {
            endReached = true;
            StopAllMovement();
            float moveDirection = Mathf.Sign(movement.x); // Get -1, 0, or 1 based on the direction of movement
            float moveSpeed = 5.0f; // Example move speed, adjust as necessary
            endMove = new Vector2(moveDirection * .5f, 10); // Set endMove to move in the direction of impact

            StartCoroutine(PlayEndingAnimation());
        }
    }

    private void StopAllMovement()
    {
        isMoving = false;
        StopAllCoroutines();
        rb.velocity = Vector2.zero; // Stop any existing movement
        rb.angularVelocity = 0; // Stop any rotation
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
    }
    IEnumerator PlayEndingAnimation()
    {
        //box.GetComponent<BoxCollider2D>().isTrigger = true;
        rb.velocity = endMove;  // Make sure 'endMove' is defined and set appropriately elsewhere in your script
        isMoving = false;

        foreach (var frame in endingSprites)
        {
            spriteRenderer.sprite = frame;
            yield return new WaitForSeconds(.2f);
        }
        gameObject.SetActive(false);
    }

    void UpdateMovementAnimation(bool moving)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        if (moving)
        {
            currentAnimation = StartCoroutine(AnimatePlayer(sideSprites));
        }
    }

    IEnumerator AnimatePlayer(Sprite[] animationFrames)
    {
        while (true)
        {
            foreach (Sprite frame in animationFrames)
            {
                spriteRenderer.sprite = frame;
                yield return new WaitForSeconds(1f / framesPerSecond);
            }
        }
    }
}
