using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingToNextScene : MonoBehaviour
{
    private bool _sceneCanChange = true;
    [SerializeField]
    private GameObject _Text;
    public FadeOutTransitionScreen fadeOutTransitionScreen;
    private void OnTriggerStay(Collider other)
    {
        if (!_sceneCanChange) return;

        if (other.CompareTag("Player"))
        {
            _Text.SetActive(true);
            PlayerInputs _inputs = other.GetComponent<PlayerInputs>();
            if (_inputs == null || !_inputs.interact)
            {
                return;
            }
            _sceneCanChange = false;
            fadeOutTransitionScreen.FadeIn();
            StartCoroutine(LoadNextSceneWithDelay());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Text.SetActive(false);
        }
    }
    private IEnumerator LoadNextSceneWithDelay()
    {
        // Load the next scene after a delay
        _Text.SetActive(false);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
