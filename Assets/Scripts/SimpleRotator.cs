using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    private float rotationSpeed = 2f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = new Vector3(0f, rotationSpeed, 0f);
    }
    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.up * rotationSpeed;
    }
}
