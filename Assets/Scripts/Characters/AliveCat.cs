using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AliveCat : MonoBehaviour
{
    public AudioClip[] jumpSounds;  // Array to hold jump sound clips
    public AudioClip hitSound, victorySound;
    private AudioSource audioSource;
    public float framesPerSecond, frameDuration, distanceFromGround, jumpForce, raycastDif, knockbackForce;
    public float moveSpeed = 5f;
    public float fallForce = 10f;
    public Sprite[] standing, licking, sideSprites, frames;
    public LayerMask groundLayer;
    public Sprite[] endingSprites;
    public float endingAnimationDuration = 5f;
    public GameObject box;
    private Vector2 endMove;
    public HealthUI Health;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Coroutine currentAnimation;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool endReached = false;
    private bool enemyHit = false;
    private bool controlAllowed = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        StartIdleAnimationCycle();
    }


    void Update()
    {
        if (!endReached && controlAllowed)
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
        if (!endReached && !enemyHit && controlAllowed)
        {
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector2.down * fallForce * 2f);
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector2.down * fallForce, ForceMode2D.Impulse);
            }
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
            audioSource.PlayOneShot(jumpSounds[index]);  // Play the clip
        }
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
            audioSource.PlayOneShot(hitSound);
            StartCoroutine(knockBack(collision));
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            audioSource.PlayOneShot(victorySound);
            controlAllowed = false;
            float moveDirection = Mathf.Sign(movement.x); // Get -1, 0, or 1 based on the direction of movement
            float moveSpeed = 5.0f; // Example move speed, adjust as necessary
            endMove = new Vector2(moveDirection * 10, 5); // Set endMove to move in the direction of impact

            StartCoroutine(PlayEndingAnimation());
        }
    }

    IEnumerator knockBack(Collision2D collision)
    {
        enemyHit = true;
        Health.DecreaseHealth();
        StaticVars.heartNums--;
        Vector2 enemyPosition = collision.transform.position;
        Vector2 knockbackDirection = (rb.position - enemyPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.14f);
        rb.velocity = Vector2.zero;
        enemyHit = false;
    }
    IEnumerator PlayEndingAnimation()
    {
        box.GetComponent<BoxCollider2D>().isTrigger = true;
        rb.velocity = endMove;
        isMoving = false;

        foreach (var frame in endingSprites)
        {
            spriteRenderer.sprite = frame;
            yield return new WaitForSeconds(.5f);
        }
        StaticVars.level++;
        NextLevel();
    }

    void RestartLevel()
    {
        // For example, reload the current scene
        Debug.Log("Restarting level");
        StaticVars.heartNums = 4;
        Debug.Log("heartNums is " + StaticVars.heartNums);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void NextLevel()
    {
        if (StaticVars.level >= 5)
        {
            StaticVars.heartNums = 4;
            StaticVars.level = 1;
            SceneManager.LoadScene("Game Won");
        }
        else
        {
            StaticVars.heartNums = 4;
            SceneManager.LoadScene("Level " + StaticVars.level);
        }
    }
}