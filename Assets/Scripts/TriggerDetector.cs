using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField] private DogPatrol dogPatrolScript; // Referensi ke skrip utama

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dogPatrolScript == null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            dogPatrolScript.isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (dogPatrolScript == null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            dogPatrolScript.isChasing = false;
        }
    }
}
