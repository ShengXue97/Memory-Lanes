using UnityEngine;

public class Enemy : MonoBehaviour
{
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
        var tf = transform;
        rb.MovePosition(tf.position + Time.deltaTime * movementSpeed * tf.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        movementSpeed *= -1; // bounce back on collision
    }
}
