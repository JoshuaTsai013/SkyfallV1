using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pauseMenuUI;
    [SerializeField] private FadeOutTransitionScreen _fadeOutTransitionScreen;
    [SerializeField] private Selectable _firstSelectedButton;
    private bool _isPaused = false;
    private bool _canToggle = true; // Add a flag to control toggling
    private ThirdPersonController _playerController;
    private ThirdPersonShooterController _playerShooterController;
    private PlayerInput _playerInput;

    private void Start()
    {
        _playerController = PlayerManager.instance.player.GetComponent<ThirdPersonController>();
        if (_playerController == null)
        {
            Debug.LogError("ThirdPersonController component not found on player.");
        }

        _playerShooterController = PlayerManager.instance.player.GetComponent<ThirdPersonShooterController>();
        if (_playerShooterController == null)
        {
            Debug.LogError("ThirdPersonShooterController component not found on player.");
        }

        _playerInput = PlayerManager.instance.player.GetComponent<PlayerInput>();
        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on player.");
        }

        _pauseMenuUI.gameObject.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if (!_canToggle) return; // Prevent toggling if not allowed
        if (!_isPaused)
        {
            OpenPauseMenu();
            Debug.Log("Opening Pause Menu");
        }
        else
        {
            ClosePauseMenu();
            Debug.Log("Closing Pause Menu");
        }
        StartCoroutine(ToggleDelay()); // Start the delay coroutine
    }
    private IEnumerator ToggleDelay()
    {
        _canToggle = false; // Disable toggling
        yield return new WaitForSecondsRealtime(0.2f); // Wait for 0.2 seconds in real-time
        _canToggle = true; // Enable toggling
    }
    public void OpenPauseMenu()
    {
        Debug.Log("Try Opening Pause Menu");
        if (_isPaused)
        {
            return;
        }
        _playerInput.enabled = false; // Disable player input actions
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton.gameObject);
        _playerController.enabled = false;
        _playerShooterController.enabled = false;
        _pauseMenuUI.DOFade(1, 0.05f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Time.timeScale = 0.1f;
        _pauseMenuUI.gameObject.SetActive(true);
        _isPaused = true;
        
    }
    public void ClosePauseMenu()
    {
        if (!_isPaused)
        {
            return;
        }
        _playerInput.enabled = true; // Enable player input actions
        _playerShooterController.enabled = true;
        _pauseMenuUI.DOFade(0, 0.05f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        StartCoroutine(ResumeGameDelay());
        _pauseMenuUI.gameObject.SetActive(false);
        _isPaused = false;
    }

    private IEnumerator ResumeGameDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f); // Wait for 0.2 seconds in real-time
        _playerController.enabled = true;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameDelay()); // Start the delay coroutine
    }

    private IEnumerator RestartGameDelay()
    {
        ClosePauseMenu();
        _fadeOutTransitionScreen.FadeIn();
        yield return new WaitForSecondsRealtime(3.0f); // Wait for 0.2 seconds in real-time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameDelay()); // Start the delay coroutine
    }

    private IEnumerator QuitGameDelay()
    {
        ClosePauseMenu();
        _fadeOutTransitionScreen.FadeIn();
        yield return new WaitForSecondsRealtime(3.0f); // Wait for 0.2 seconds in real-time
        // SceneManager.LoadScene(0);
        Application.Quit();
    }
}
