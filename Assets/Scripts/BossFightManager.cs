using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    private static WaitForSecondsRealtime _waitForSecondsRealtime5_0 = new(5.0f);
    private static WaitForSecondsRealtime _waitForSecondsRealtime1 = new(1f);
    public CanvasGroup BossBloodBar;
    public Image BloodBarFill;
    [SerializeField] private CharacterGeneral _boss;
    [SerializeField] private FadeOutTransitionScreen _fadeOutTransitionScreen;
    [SerializeField] private AudioSource _bossMusic;
    [SerializeField] private ParticleSystem _bossAreaSmoke;
    [SerializeField] private Transform _playerRespawnPoint;
    [SerializeField] private GameObject AmmoKit;
    [SerializeField] private GameObject RepairKit;

    private CharacterGeneral _characterGeneral;
    private bool _showBloodBar = false;
    private bool _isBossDead = false;
    public Aimbot _aimbot;
    private Coroutine _bloodBarCoroutine;

    // Saved pre-fight state
    private Vector3 _bossInitialPosition;
    private Quaternion _bossInitialRotation;
    private float _bossInitialHealth;
    private Vector3 _aimbotInitialTargetOffset;
    private float _aimbotInitialScreenScanRadius;

    private void Start()
    {
        if (_boss)
        {
            _boss.OnDie.AddListener(HandleDie);
            _bossInitialPosition = _boss.transform.position;
            _bossInitialRotation = _boss.transform.rotation;
            _bossInitialHealth = _boss.maxHealth;
        }
        if (_aimbot)
        {
            _aimbotInitialTargetOffset = _aimbot.targetOffset;
            _aimbotInitialScreenScanRadius = _aimbot.screenScanRadius;
        }
        BloodBarFill.fillAmount = 1;
        BossBloodBar.alpha = 0f;
        BossBloodBar.gameObject.SetActive(false);
    }

    // Start Boss Fight on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_showBloodBar)
        {
            // Now safe to refresh the spawn override with current entrance position
            // (previous override kept the player safe during respawn after last death)
            Vector3 spawnPos = _playerRespawnPoint != null ? _playerRespawnPoint.position : transform.position;
            Quaternion spawnRot = _playerRespawnPoint != null ? _playerRespawnPoint.rotation : transform.rotation;
            PlayerRespawn.SetSpawnOverride(spawnPos, spawnRot);

            // Lock respawn to arena entrance as fixed values — immune to checkpoint overrides
            Transform respawnTr = _playerRespawnPoint != null ? _playerRespawnPoint : transform;
            PlayerRespawn.SetSpawnOverride(respawnTr.position, respawnTr.rotation);
            StartBossFight();
            BossBloodBar.gameObject.SetActive(true);
            BossBloodBar.DOFade(1, 2f);
            _showBloodBar = true;
            _bossMusic.Play();
            _bossAreaSmoke.Play();
            _aimbot.targetOffset = new Vector3(0, 8.3f, 0);
            _aimbot.screenScanRadius = 0.5f;

            _characterGeneral = other.GetComponent<CharacterGeneral>();
            if (_characterGeneral)
            {
                _characterGeneral.OnDie.AddListener(PlayerDie);
            }
            // Debug.Log("Boss Fight Started!");
        }
    }

    private void StartBossFight()
    {
        StopAllCoroutines();
        _isBossDead = false;
        _bloodBarCoroutine = StartCoroutine(UpdateBossBloodBar());
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
         if (_characterGeneral)
            {
                _characterGeneral.currentHealth = 100f;
            }
        StartCoroutine(EndGameDelay());
    }

    // Handle player death: wait 1 seconds then restore pre-fight state
    private void PlayerDie()
    {
        // Debug.Log("Player died during boss fight - resetting boss fight in 1 seconds");

        // Unsubscribe immediately to prevent duplicate calls
        if (_characterGeneral != null)
        {
            _characterGeneral.OnDie.RemoveListener(PlayerDie);
            _characterGeneral = null;
        }

        StartCoroutine(ResetBossFightAfterDelay());
    }

    private IEnumerator ResetBossFightAfterDelay()
    {
        yield return _waitForSecondsRealtime1;

        if (_bloodBarCoroutine != null)
        {
            StopCoroutine(_bloodBarCoroutine);
            _bloodBarCoroutine = null;
        }
        if (_boss)
        {
            _boss.transform.SetPositionAndRotation(_bossInitialPosition, _bossInitialRotation);
            _boss.currentHealth = _bossInitialHealth;
        }
        if (_aimbot)
        {
            _aimbot.targetOffset = _aimbotInitialTargetOffset;
            _aimbot.screenScanRadius = _aimbotInitialScreenScanRadius;
        }

        // Stop music
        _bossMusic.Stop();
        _bossMusic.time = 0f;
        _bossAreaSmoke.Stop();

        // Hide blood bar
        BossBloodBar.DOKill();
        BossBloodBar.alpha = 0f;
        BossBloodBar.gameObject.SetActive(false);
        BloodBarFill.fillAmount = 1f;

        // respawn ammo box and health pack
        Instantiate(AmmoKit, new Vector3(348,401,269), Quaternion.identity);
        Instantiate(RepairKit, new Vector3(344,401,260.6f), Quaternion.identity);
        
        // Reset flags — allow OnTriggerEnter to fire again
        _showBloodBar = false;
        _isBossDead = false;

    }

    private Vector3 Vector3(double v1, double v2, double v3)
    {
        throw new NotImplementedException();
    }

    private IEnumerator EndGameDelay()
    {
        BossBloodBar.DOFade(0, 0.2f);
        _fadeOutTransitionScreen.FadeIn();
        yield return _waitForSecondsRealtime5_0;
        SceneManager.LoadScene(4);
    }
}
