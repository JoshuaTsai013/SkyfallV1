using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    public CanvasGroup BossBloodBar;
    public Image BloodBarFill;
    [SerializeField] private CharacterGeneral _boss;
    [SerializeField] private FadeOutTransitionScreen _fadeOutTransitionScreen;
    [SerializeField] private AudioSource _bossMusic;
    private bool _showBloodBar = false;
    private bool _isBossDead = false;
    private void Start()
    {
        if (_boss)
        {
            _boss.OnDie.AddListener(HandleDie);
        }
        BloodBarFill.fillAmount = 1;
        BossBloodBar.alpha = 0f;
        BossBloodBar.gameObject.SetActive(false);
    }
    //start Boss Fight
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_showBloodBar)
        {
            StartBossFight();
            BossBloodBar.gameObject.SetActive(true);
            BossBloodBar.DOFade(1, 2f);
            _showBloodBar = true;
            _bossMusic.Play();
            Debug.Log("Boss Fight Started!");
        }
    }

    private void StartBossFight()
    {
        StartCoroutine(UpdateBossBloodBar());
    }
    private IEnumerator UpdateBossBloodBar()
    {
        while (!_isBossDead)
        {
            BloodBarFill.fillAmount = _boss.HealthPercentage;
            yield return null;
        }
    }
    private void HandleDie()
    {
        _isBossDead = true;
        StartCoroutine(EndGameDelay());
    }
    private IEnumerator EndGameDelay()
    {
        BossBloodBar.DOFade(0, 0.2f);
        _fadeOutTransitionScreen.FadeIn();
        yield return new WaitForSecondsRealtime(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
