using UnityEngine;

public class DestroySelfTime : MonoBehaviour
{
    [SerializeField] private float destroyTime = 5f; // Time in seconds before the object is destroyed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime); // Destroys the GameObject after 5 seconds
    }

}
