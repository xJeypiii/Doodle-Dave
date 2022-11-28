using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private int frame;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;

            if (Input.GetButton("Jump") && IsCrawling()) {
                direction = Vector3.up * jumpForce;
                GetComponent<Animator>().Play("JumpAnim");
            }
        }

        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) {
            FindObjectOfType<GameManager>().GameOver(other);
        }
    }

    private bool IsCrawling()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CrawlAnim"))
        {
            return true;
        }

        return false;
    }
}
