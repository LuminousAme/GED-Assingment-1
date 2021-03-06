using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //serialized field so we can modify it from the editor
    //the speed of the player's movement
    [SerializeField] private float moveSpeed = 10f;

    //layermask that tells us what the player can jump off of 
    [SerializeField] private LayerMask jumpMask;

    //controls for the jump
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float horiDistanceToPeak = 1f;
    [SerializeField] private float horiDistanceWhileFalling = 0.5f;
    private float jumpInitialVerticalVelo;
    private float gravityGoingUp;
    private float gravityGoingDown;
    private bool startJump;

    //enemy collision force
    [SerializeField] private float enemyCollisionKnockback = 10f;

    //rigidbody
    private Rigidbody rb;
    //the player's horizontal input
    private float xinput = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //grab a reference to the rigidbody
        rb = this.GetComponent<Rigidbody>();

        //jump calculations based on the building a better jump GDC talk, source: https://youtu.be/hG9SzQxaCm8

        //use that along with the desired height, movement speed, and the disctance to peak to find the intial vertical velocity for the jump
        jumpInitialVerticalVelo = (2f * jumpHeight * moveSpeed) / horiDistanceToPeak;
        //calculate the gravity using the same variables (note two different gravities to allow for enhanced game feel)
        gravityGoingUp = (-2f * jumpHeight * (moveSpeed * moveSpeed) / (horiDistanceToPeak * horiDistanceToPeak));
        gravityGoingDown = (-2f * jumpHeight * (moveSpeed * moveSpeed) / (horiDistanceWhileFalling * horiDistanceWhileFalling));

        //set the default gravity to be the gravity going up 
        Physics.gravity = Vector3.down * gravityGoingUp;

        startJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        //grab the input from the player for movement
        xinput = Input.GetAxis("Horizontal");

        //if the player has pressed the space key this frame and can jump, then jump
        if (Input.GetKeyDown(KeyCode.Space) && checkIsGrounded()) startJump = true;
    }

    // FixedUpdate is called once per physics step
    private void FixedUpdate()
    {
        //set gravity based on player velocity
        if (rb.velocity.y > 0) Physics.gravity = Vector3.up * gravityGoingUp;
        else if (rb.velocity.y < 0) Physics.gravity = Vector3.up * gravityGoingDown;

        //if the player presses jump and they can jump
        if (startJump) rb.velocity = new Vector3(0f, jumpInitialVerticalVelo, 0f);
        startJump = false;

        //move the player on the horizontal axis based on the player input
        rb.MovePosition(rb.position + moveSpeed * new Vector3(0.0f, 0.0f, xinput) * Time.fixedDeltaTime);
    }

    //check if the player is grounded using a raycast
    //based on code from the Code Monkey, Source: https://youtu.be/c3iEl5AwUF8 
    private bool checkIsGrounded()
    {
        //do the boxcast 
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        RaycastHit hit;
        bool detect = Physics.Raycast(rb.position, Vector3.down, out hit, 1.1f * transform.localScale.y, jumpMask);

        //return if it hit anything
        return (hit.collider != null);
    }

    //collision with enemy
    private void OnCollisionEnter(Collision collision)
    {
        //only process the collision if it's with an enemy
        if (collision.gameObject.layer == 7)
        {
            //get the contact point
            ContactPoint contact = collision.contacts[0];

            //knock the player back
            rb.MovePosition(contact.point + contact.normal.normalized * enemyCollisionKnockback);

            //screenshake
            Camera.main.GetComponent<CameraFollow>().shakeScreen(0.25f, 0.2f);

            //subtract from the player's hp
            FindObjectOfType<GameManager>().ChangeHp(-0.05f);
        }
    }

    //collision with enemy hurtbox
    private void OnTriggerEnter(Collider other)
    {
        //if the trigger the player collided with is in fact an enemy's hurtbox, destory that enemy
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            FindObjectOfType<GameManager>().DropEnemyNum();
        }
    }
}