using UnityEngine;
using UnityEngine.Events;

public class TrainingTrigger : MonoBehaviour
{    public bool stay = false;

    [Header("Trigger Events")]
    [Tooltip("Fires when the player enters the trigger area.")]
    public UnityEvent onTriggerEnterEvent;
    
    [Tooltip("Fires when the player exits the trigger area.")]
    public UnityEvent onTriggerExitEvent;

    [Tooltip("If true, the enter event will only fire the first time the player enters.")]
    public bool triggerOnce = false;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!triggerOnce || !_hasTriggered)
            {
                onTriggerEnterEvent?.Invoke();
                _hasTriggered = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            stay = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stay = false;
            onTriggerExitEvent?.Invoke();
        }
    }
}
