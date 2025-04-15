using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battleshipmanager : MonoBehaviour
{
    public CanvasGroup fadeOutTransitionScreen; // Reference to the fade-out transition screen
    private void Start()
    {
        StartCoroutine(LoadNextSceneWithDelay());
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        // Load the next scene after a delay
        yield return new WaitForSeconds(20f); // Adjust the delay as needed
        fadeOutTransitionScreen.DOFade(1, 2.5f);
        yield return new WaitForSeconds(3f); // Wait for the fade-out to complete
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
    }
}
