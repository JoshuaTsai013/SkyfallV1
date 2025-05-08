using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battleshipmanager : MonoBehaviour
{
    public FadeOutTransitionScreen fadeOutTransitionScreen; // Reference to the fade-out transition screen
    public void LoadNextScene()
    {
        // Start the coroutine to load the next scene
        StartCoroutine(LoadNextSceneWithDelay());
    }
    private IEnumerator LoadNextSceneWithDelay()
    {
        fadeOutTransitionScreen.FadeIn(); // Fade in the transition screen
        yield return new WaitForSeconds(3f); // Wait for the fade-out to complete
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
    }
}
