using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject zoneToEnable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnableZone(zoneToEnable);
            Destroy(gameObject); 
        }
    }

    void EnableZone(GameObject zone)
    {
        foreach (Rigidbody rb in zone.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            Collider col = rb.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;
        }
    }
}
