using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}
