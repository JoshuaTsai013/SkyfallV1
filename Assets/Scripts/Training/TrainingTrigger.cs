using UnityEngine;

public class TrainingTrigger : MonoBehaviour
{    public bool stay = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        stay = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        stay = false;
    }
}
