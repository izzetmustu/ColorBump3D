using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LevelManager levelManager;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    [Header("Velocity variables")]
    [SerializeField] float minVelocityZ = 1f;
    [SerializeField] float maxVelocityZ = 5f;
    [SerializeField] float maxDragSpeed = 25f;
    [SerializeField] float acceleration = 1f;


    private Rigidbody rb;
    private float currentVelocityZ = 0f;
    private bool isBeingDragged  = false;
    private bool isGameOver = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isGameOver)
        {
            Vector3 velocity = rb.linearVelocity;
            float deceleration = acceleration * Time.fixedDeltaTime;

            if (velocity.magnitude > 0.01f)
            {
                Vector3 decelerated = velocity - velocity.normalized * deceleration;

                if (Vector3.Dot(velocity, decelerated) < 0f)
                    decelerated = Vector3.zero;

                rb.linearVelocity = decelerated;
                currentVelocityZ = decelerated.z;
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
                currentVelocityZ = 0f;
            }          
        }
        else
        {
            if (!isBeingDragged)
            {
                if (currentVelocityZ > maxVelocityZ)
                {
                    currentVelocityZ = currentVelocityZ - acceleration * Time.fixedDeltaTime;
                }
                else
                {
                    currentVelocityZ = currentVelocityZ + acceleration * Time.fixedDeltaTime;
                }
                // rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, currentVelocityZ);
                float clampedZ = Mathf.Max(currentVelocityZ, minVelocityZ);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, clampedZ);
                currentVelocityZ = clampedZ;
            }
            levelManager.UpdateProgress((transform.position.z - startPoint.transform.position.z)/(endPoint.transform.position.z - startPoint.transform.position.z));            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isGameOver = true;
            levelManager.OnGameOver();
        }
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        Vector3 lerped = Vector3.Lerp(rb.linearVelocity, newVelocity, 0.25f);

        Vector3 clampedVelocity = ClampVelocity(lerped);
        rb.linearVelocity = clampedVelocity;
        currentVelocityZ = clampedVelocity.z;
    }

    private Vector3 ClampVelocity(Vector3 newVelocity)
    {
        Vector3 resultVelocity = newVelocity;
        if (newVelocity.magnitude > maxDragSpeed)
        {
            resultVelocity = newVelocity.normalized * maxDragSpeed;
        }
        return resultVelocity;
    }

    public void OnStartDrag()
    {
        isBeingDragged = true;
    }
    public void OnEndDrag()
    {
        isBeingDragged = false;
    } 
}
