using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 directionVector = Vector3.forward;
    public float movementSpeed = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(transform.position + directionVector.normalized * Time.deltaTime * movementSpeed);
        //transform.Translate(directionVector.normalized * Time.deltaTime * movementSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            directionVector.z *= -1;
            directionVector.x *= -1;
        }

        if (collision.gameObject.tag == "aPlayer")
        {
            directionVector.z *= -1; // bounce back on collision
            directionVector.x *= -1;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        /*if (collision.gameObject.tag == "aPlayer")
        {
            isStuck = false;
        }*/
    }
}
