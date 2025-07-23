using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.OnLevelFinish();
        }
    }
}
