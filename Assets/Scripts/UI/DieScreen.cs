using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement; // Import DOTween namespace

public class DieScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // Reference to the CanvasGroup

    private void Start()
    {
        canvasGroup.gameObject.SetActive(false); // Initially hide the die screen
    }

    public void ShowDieScreen()
    {
        if (canvasGroup != null)
        {
           canvasGroup.gameObject.SetActive(true);
            // Set initial alpha to 0 and fade in over 0.6 seconds
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.6f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
         else
        {
            Debug.LogError("CanvasGroup is not assigned.");
        }
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameDelay()); // Start the delay coroutine
    }

    private IEnumerator RestartGameDelay()
    {
        canvasGroup.DOFade(0, 2.9f);
        yield return new WaitForSecondsRealtime(3.0f); // Wait for 3 seconds in real-time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
