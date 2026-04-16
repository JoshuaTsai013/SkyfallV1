using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.VFX;

public class LoadingScreen : MonoBehaviour
{
    private static readonly WaitForSecondsRealtime _waitForSecondsRealtime3_0 = new(3.0f);
    public CanvasGroup loadingScreen;
    public CanvasGroup loadingText;
    public VisualEffect loadingVfx;
    public ParticleSystem StartParticles;
    public AudioSource buttonSound;
    public CanvasGroup Title;
    public CanvasGroup StartText;
    [SerializeField]
    private int sceneIndex;

    private void Start()
    {
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(true, CursorLockMode.None);
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        RunTitle(destroyCancellationToken).Forget();
        loadingVfx.Stop();
    }
    async UniTask RunTitle(CancellationToken cancellation)
    {
        Title.DOFade(1, 2f);
        await UniTask.Delay(1000, cancellationToken: cancellation);
        StartParticles.gameObject.SetActive(true);
        await UniTask.Delay(1000, cancellationToken: cancellation);
        StartText.DOFade(1, 1f);
        await UniTask.Delay(1000, cancellationToken: cancellation);
    }
    public void StartGame()
    {
        buttonSound.Play();
        RunToLoading(destroyCancellationToken).Forget();
    }
    async UniTask RunToLoading(CancellationToken cancellation)
    {
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(false, CursorLockMode.Locked);
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        StartParticles.Stop();
        StartText.DOFade(0, 0.6f);
        Title.DOFade(0, 1f);
        loadingScreen.DOFade(1, 2f);
        await UniTask.Delay(1000, cancellationToken: cancellation);
        loadingText.DOFade(1, 1f);
        loadingVfx.Play();
        await UniTask.Delay(1000, cancellationToken: cancellation);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        yield return null;
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameDelay()); // Start the delay coroutine
    }

    private IEnumerator QuitGameDelay()
    {
        Title.DOFade(0, 0.5f);
        StartText.DOFade(0, 0.5f);
        yield return _waitForSecondsRealtime3_0;
        Application.Quit();
    }
}
