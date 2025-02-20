using System.Collections;
using UnityEngine;

public class TrainingDoorOpen : MonoBehaviour
{
    public AudioSource audioSource;
    public float speed = 0.001f;
    public void OpenDoor()
    {
        StartCoroutine(Open());
        audioSource.PlayOneShot(audioSource.clip);
        Debug.Log("Open Door");
    }

    IEnumerator Open()
    {
        while (true)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
            yield return null;
        }
    }
}
