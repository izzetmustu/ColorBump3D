using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            Debug.Log("Destroyed obstacle: " + collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    } 
}
