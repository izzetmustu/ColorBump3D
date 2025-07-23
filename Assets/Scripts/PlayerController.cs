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
    private Vector3 dragStartPoint;
    private Vector3 playerStartPoint;
    private Vector2 dragStartScreen;
    private float dragThreshold;
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
        Vector2 screenPosition = touchPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null) // && (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Ground")))
            {
                float initialDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
                dragStartScreen = screenPosition;
                dragStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, initialDistance));
                playerStartPoint = playerMovement.transform.position;
                dragThreshold = playerMovement.rb.linearVelocity.magnitude;
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        playerMovement.OnStartDrag();

        float initialDistance = Vector3.Distance(clickedObject.transform.position, Camera.main.transform.position);
        while (touchPressed.ReadValue<float>() != 0)
        {
            Vector2 screenPos = touchPosition.ReadValue<Vector2>();
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, initialDistance));
            Vector3 playerCurrentPosition = playerMovement.transform.position;
            if (Vector2.Distance(screenPos, dragStartScreen) > 2f)
            {
                Vector3 direction = currentPosition - dragStartPoint - (playerCurrentPosition - playerStartPoint);   // Calculate direction to move
                direction.y = 0f;   // Zero out y, it should not go up and down
                playerMovement.SetVelocity(direction * dragSpeed);
            }

            yield return waitForFixedUpdate;
        }
        
        playerMovement.OnEndDrag();
    }
}
