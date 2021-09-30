using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //serialized field so we can modify it from the editor
    //the speed of the player's movement
    [SerializeField] private float moveSpeed = 10f;

    //rigidbody
    private Rigidbody rb;
    //the player's horizontal input
    private float xinput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //grab a reference to the rigidbody
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //grab the input from the player for movement
        xinput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once per physics step
    private void FixedUpdate()
    {
        //move the player on the horizontal axis based on the player input
        rb.MovePosition(rb.position + moveSpeed * new Vector3(0.0f, 0.0f, xinput) * Time.fixedDeltaTime);
    }
}
