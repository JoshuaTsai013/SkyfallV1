using System.Collections;
using UnityEngine;

public class TrainingDoorClose : MonoBehaviour
{
    public AudioSource audioSource;
    public float speed = 1f;
    public float closeDuration = 2.5f; // How long the door takes to close
    private bool _isMoving = false;
    
    public void CloseDoor()
    {
        if (_isMoving) return;

        StartCoroutine(Close());
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    IEnumerator Close()
    {
        _isMoving = true;
        float timer = 0f;
        while (timer < closeDuration)
        {
            transform.Translate(-1f * speed * Time.deltaTime * Vector3.forward);
            timer += Time.deltaTime;
            yield return null;
        }
        _isMoving = false;
    }
}
