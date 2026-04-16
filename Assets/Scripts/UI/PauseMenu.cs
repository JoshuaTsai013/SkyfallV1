using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private static WaitForSecondsRealtime _waitForSecondsRealtime0_2 = new WaitForSecondsRealtime(0.2f);
    private static readonly WaitForSecondsRealtime _waitForSecondsRealtime3_0 = new(3.0f);
    [SerializeField] private CanvasGroup _pauseMenuUI;
    [SerializeField] private FadeOutTransitionScreen _fadeOutTransitionScreen;
    [SerializeField] private Selectable _firstSelectedButton;
    private bool _isPaused = false;
    private bool _canToggle = true; // Add a flag to control toggling
    [SerializeField]
    private ThirdPersonController _playerController;
    [SerializeField]
    private ThirdPersonShooterController _playerShooterController;
    [SerializeField]
    private PlayerInput _playerInput;

    private void Awake()
    {
        _pauseMenuUI.gameObject.SetActive(false);
    }

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
        yield return _waitForSecondsRealtime0_2; // Wait for 0.2 seconds in real-time
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
        // _playerInput.SwitchCurrentActionMap("UI"); // Switch to the UI action map
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton.gameObject);
        _playerController.enabled = false;
        _playerShooterController.enabled = false;
        _pauseMenuUI.DOFade(1, 0.05f);
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(true, CursorLockMode.None);
        Time.timeScale = 0.1f;
        _pauseMenuUI.gameObject.SetActive(true);
        _isPaused = true;

    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && _isPaused)
        {
            StartCoroutine(ReapplyPauseCursorNextFrame());
        }
    }

    private IEnumerator ReapplyPauseCursorNextFrame()
    {
        // Reapply once immediately and once next frame in case another script overrides cursor state on focus regain.
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(true, CursorLockMode.None);
        yield return null;
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(true, CursorLockMode.None);
    }

    public void ClosePauseMenu()
    {
        if (!_isPaused)
        {
            return;
        }
        _playerInput.enabled = true; // Enable player input actions
        // _playerInput.SwitchCurrentActionMap("Player"); // Switch back to the player action map
        _playerShooterController.enabled = true;
        _pauseMenuUI.DOFade(0, 0.05f);
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(false, CursorLockMode.Locked);
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
        yield return _waitForSecondsRealtime3_0;
        SceneManager.LoadScene(0);
        // Application.Quit();
    }
}
