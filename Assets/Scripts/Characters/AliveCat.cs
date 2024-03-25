using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AliveCat : MonoBehaviour
{
    public float framesPerSecond, frameDuration, distanceFromGround, jumpForce;
    public float moveSpeed = 5f;
    public Sprite[] standing, licking, sideSprites, frames;
    public LayerMask groundLayer;
    public Sprite[] endingSprites;
    public float endingAnimationDuration = 5f;
    public Vector2 endMove;
    public HealthUI Health;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Coroutine currentAnimation;
    private bool isMoving = false;
    private bool endReached = false;
    // private Vector3 targetFinalPosition = new Vector3(1350.23f, -102.537f, 23.89023f);


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartIdleAnimationCycle();
    }


    void Update()
    {
        if (!endReached)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();

            if (Mathf.Abs(movement.x) > 0 && !isMoving) // Starts moving
            {
                isMoving = true;
                UpdateMovementAnimation(true);
            }
            else if (Mathf.Abs(movement.x) == 0 && isMoving) // Stops moving
            {
                isMoving = false;
                StartIdleAnimationCycle(); // Go back to idle animations
            }
        }

    }

    void FixedUpdate()
    {
        if (!endReached)
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround, groundLayer);
        return hit.collider != null;
    }

    void Jump()
    {
        if (IsGrounded())
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void UpdateMovementAnimation(bool moving)
    {
        if (moving && !endReached)
        {
            StopIdleAnimationCycle();
            if (movement.x > 0)
            {
                spriteRenderer.flipX = false; // No flipping for right animation
            }
            else if (movement.x < 0)
            {
                spriteRenderer.flipX = true; // Flip for left animation
            }
            StartMovingAnimationCoroutine(sideSprites);
        }
    }

    void StartMovingAnimationCoroutine(Sprite[] animationFrames)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimatePlayer(animationFrames));
    }

    IEnumerator AnimatePlayer(Sprite[] animationFrames)
    {
        while (true)
        {
            foreach (var frame in animationFrames)
            {
                spriteRenderer.sprite = frame;
                yield return new WaitForSeconds(1f / framesPerSecond);
            }
        }

    }

    public void StartIdleAnimationCycle()
    {
        StopIdleAnimationCycle();
        currentAnimation = StartCoroutine(AnimateStandingLickingCycle());
    }

    IEnumerator AnimateStandingLickingCycle()
    {
        while (!isMoving) // Keep cycling through animations only if not moving
        {
            yield return AnimateSprites(standing, 4f); // Play standing animation for 4 seconds
            yield return AnimateSprites(licking, 3f); // Play licking animation for 3 seconds
        }
    }

    IEnumerator AnimateSprites(Sprite[] animationFrames, float duration)
    {
        float timePerFrame = duration / animationFrames.Length;
        foreach (var frame in animationFrames)
        {
            spriteRenderer.sprite = frame;
            yield return new WaitForSeconds(timePerFrame);
        }
    }

    public void StopIdleAnimationCycle()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is on the Enemies layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Health.DecreaseHealth();
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            endReached = true;
            StopAllCoroutines();
            StartCoroutine(PlayEndingAnimation());
        }

    }

    IEnumerator PlayEndingAnimation()
    {
        rb.velocity = endMove;
        isMoving = false;

       foreach (var frame in endingSprites)
    {
        spriteRenderer.sprite = frame;
        yield return new WaitForSeconds(1/frameDuration);
    }
        RestartLevel();
    }

    void RestartLevel()
    {
        // For example, reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}