using UnityEngine;
using DG.Tweening;

public class DieScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Reference to the CanvasGroup

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
            canvasGroup.DOFade(1, 3f);
        }
        else
        {
            Debug.LogError("CanvasGroup is not assigned.");
        }
    }
}
