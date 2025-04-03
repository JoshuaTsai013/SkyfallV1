using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.VFX;

public class LoadingScreen : MonoBehaviour
{
    public CanvasGroup loadingScreen;
    public CanvasGroup loadingText;
    public VisualEffect loadingVfx;
    public ParticleSystem StartParticles;
    public AudioSource buttonSound;
    public CanvasGroup Title;
    public CanvasGroup StartText;
    [SerializeField]
    private int sceneIndex;
    private bool sceneCanChange = false;
    private void Start()
    {
        RunTitle(destroyCancellationToken).Forget();
        loadingVfx.Stop();
        sceneCanChange = true;
    }
    async UniTask RunTitle(CancellationToken cancellation)
    {
        Title.DOFade(1, 2f);
        await UniTask.Delay(1000, cancellationToken: cancellation);
        StartParticles.gameObject.SetActive(true);
        await UniTask.Delay(1000, cancellationToken: cancellation);
        StartText.DOFade(1, 1f);
        await UniTask.Delay(1000, cancellationToken: cancellation);
        sceneCanChange = true;
    }

    // private void Update()
    // {
    //     if (Input.anyKeyDown && sceneCanChange)
    //     {
    //         sceneCanChange = false;
    //         buttonSound.Play();

    //         RunToLoading(destroyCancellationToken).Forget();
    //     }
    // }
    public void StartGame()
    {
        sceneCanChange = false;
        buttonSound.Play();
        RunToLoading(destroyCancellationToken).Forget();
    }
    async UniTask RunToLoading(CancellationToken cancellation)
    {
        StartParticles.Stop();
        StartText.DOFade(0, 0.5f);
        Title.DOFade(0, 0.5f);
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
        yield return new WaitForSecondsRealtime(3.0f); // Wait for 0.2 seconds in real-time
        Application.Quit();
    }
}
