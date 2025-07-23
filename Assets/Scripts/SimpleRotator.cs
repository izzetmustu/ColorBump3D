using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    private float rotationSpeed = 2f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (rb.isKinematic) return;
        rb.angularVelocity = Vector3.up * rotationSpeed;
    }
}
