using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private SpriteRenderer spriteRenderer;
    private int frame;
    private bool isAnimating = false;

    public Sprite[] sprites;
    public Sprite readyOne;
    public Sprite readyTwo;
    public Sprite landingOne;
    public Sprite landingTwo;
    public Sprite landingThree;
    public Sprite landingFour;
    public Sprite landingFive;
    public Sprite flip;
    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        // if(!isAnimating) {
        //     transform.eulerAngles = new Vector3(0, 0, 0);
        //     Invoke(nameof(Animate), 0f);
        //     isAnimating = true;
        // }
    }

    private void OnDisable()
    {
        // CancelInvoke();
        // isAnimating = false;
    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (Input.GetButton("Jump")) {
                direction = Vector3.up * jumpForce;
                // CancelInvoke();
                // isAnimating = false;
                GetComponent<Animator>().Play("JumpAnim");
                // StartCoroutine(AnimateJump());
            }
        }

        character.Move(direction * Time.deltaTime);
    }

    IEnumerator AnimateJump()
    {
        spriteRenderer.sprite = readyOne;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = readyTwo;

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(Rotate(0.7f));
        spriteRenderer.sprite = flip;

        yield return new WaitForSeconds(0.7f);

        spriteRenderer.sprite = landingOne;

        yield return new WaitForSeconds(0.3f);

        spriteRenderer.sprite = landingTwo;

        yield return new WaitForSeconds(0.3f);

        spriteRenderer.sprite = landingThree;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = landingFour;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = landingFive;

        yield return new WaitForSeconds(0.1f);

        if(character.isGrounded) {
            if(!isAnimating) {
                Invoke(nameof(Animate), 0f);
                isAnimating = true;
            }
        }
        

    }

    IEnumerator Rotate(float duration)
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 450.0f;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 450.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation * -1);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) {
            FindObjectOfType<GameManager>().GameOver(other);
        }
    }

    private void Animate()
    {
        frame++;

        if (frame >= sprites.Length) {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }

        Invoke(nameof(Animate), 1f / 10);
    }

}
