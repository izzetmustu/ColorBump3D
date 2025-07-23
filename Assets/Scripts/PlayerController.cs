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
                dragStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, initialDistance));
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
            Vector2 screenPos = touchPosition.ReadValue<Vector2>();
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, initialDistance));
            Vector3 direction = currentPosition - dragStartPoint;   // Calculate direction to move
            Debug.Log(direction);
            direction.y = 0f;   // Zero out y, it should not go up and down
            playerMovement.SetVelocity(direction * dragSpeed);
            yield return waitForFixedUpdate;

        }
        playerMovement.OnEndDrag();
    }
}
