using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPositionCat : MonoBehaviour
{
    public AudioClip[] jumpSounds;  // Array to hold jump sound clips
    private AudioSource audioSource;
    public float framesPerSecond, distanceFromGround, jumpForce, raycastDif;
    public float moveSpeed = 5f;
    public float fallForce = 10f;
    public Sprite[] standing, licking, sideSprites, frames;
    public GameObject aliveCat, deadCat;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Coroutine currentAnimation;
    private bool isMoving = false;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        StartIdleAnimationCycle();
    }


    void Update()
    {
        if (StaticVars.superPositionActive)
        {
            StaticVars.superPositionTimer -= Time.deltaTime;
            if (StaticVars.superPositionTimer <= 0)
            {
                StaticVars.superPositionActive = false;
            }
        }
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

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        // Apply additional gravity force during ascent
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * fallForce * 2f); // Adjust this value as needed
        }
        else if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector2.down * fallForce, ForceMode2D.Impulse);
        }

    }

    bool IsGrounded()
    {
        Vector3 rightRayOrigin = transform.position + new Vector3(raycastDif, 0, 0);
        Vector3 leftRayOrigin = transform.position + new Vector3(-raycastDif, 0, 0);
        RaycastHit2D hit1 = Physics2D.Raycast(rightRayOrigin, Vector2.down, distanceFromGround, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.down, distanceFromGround, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(leftRayOrigin, Vector2.down, distanceFromGround, groundLayer);
        return (hit1.collider != null || hit2.collider != null || hit3.collider != null);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            PlayRandomJumpSound();
        }
    }

    void PlayRandomJumpSound()
    {
        if (jumpSounds.Length > 0)
        {
            int index = Random.Range(0, jumpSounds.Length);  // Get a random index
            audioSource.clip = jumpSounds[index];  // Set the clip to play
            audioSource.Play();  // Play the clip
        }
    }

    void UpdateMovementAnimation(bool moving)
    {
        if (moving)
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
}
