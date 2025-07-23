using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float dragSpeed = 10f;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private InputAction touchPosition;
    private InputAction touchPressed;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerInput = GetComponent<PlayerInput>();
        touchPosition = playerInput.actions.FindAction("TouchPosition");
        touchPressed = playerInput.actions.FindAction("TouchPress");
    }

    private void OnEnable()
    {
        touchPressed.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressed.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Debug.Log("PRESSED");
        Vector2 screenPosition = touchPosition.ReadValue<Vector2>();
        // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("HIT");
            Debug.Log(hit.collider.tag);
            if ((hit.collider != null) && hit.collider.CompareTag("Player"))
            {
                Debug.Log("HIT = THIS GAME OBJECT");
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, Camera.main.transform.position);
        playerMovement.OnStartDrag();
        while (touchPressed.ReadValue<float>() != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition.ReadValue<Vector2>()); // Update ray if we still touch on the screen
            Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;   // Calculate direction to move
            direction.y = 0f;   // Zero out y, it should not go up and down
            playerMovement.SetVelocity(direction * dragSpeed);
            // clickedObject.transform.position += direction * dragSpeed * Time.unscaledDeltaTime;
            // rb.linearVelocity = direction * dragSpeed; // 
            yield return waitForFixedUpdate;

        }
        playerMovement.OnEndDrag();
    }
}
