using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float MeleeColdDown = 1f;
    public float ChargeDuration = 1f;
    public float DrillingForwardSpeed = 1f;
    public float DrillDuration = 0.2f;
    public float MeleeRotate = 5f;
    public GameObject meleeCam;
    public GameObject mainCam;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Drill _drill;

    public bool CanMelee;
    private bool _isDrillingForward;

    private CharacterController _controller;
    private ThirdPersonController _playerController;
    private ThirdPersonShooterController _playerShooterController;
    private CinemachineImpulseSource _impulseSource;
    [SerializeField]
    private GameObject _meleeCollider;


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerController = PlayerManager.instance.player.GetComponent<ThirdPersonController>();
        _playerShooterController = PlayerManager.instance.player.GetComponent<ThirdPersonShooterController>();
        _impulseSource = PlayerManager.instance.PlayerCamera.GetComponent<CinemachineImpulseSource>();
        _meleeCollider.SetActive(false);
        CanMelee = true;
    }
    public void TryMeleeAttack()
    {
        if (CanMelee)
        {
            CanMelee = false;
            _playerController.enabled = false;
            _playerShooterController.enabled = false;
            StartCoroutine(MeleeDelay());
        }
    }

    private IEnumerator MeleeDelay()
    {
        //charging
        _animator.SetTrigger("Melee");
        meleeCam.SetActive(true);
        _impulseSource.GenerateImpulse();
        _drill.DrillStart();
        transform.Rotate(Vector3.up, MeleeRotate);
        Debug.Log("Melee Attack!!");
        yield return new WaitForSeconds(ChargeDuration);
        //drillingForward
        _meleeCollider.SetActive(true);

        _isDrillingForward = true;
        StartCoroutine(DrillingForward());
        meleeCam.SetActive(false);
        yield return new WaitForSeconds(DrillDuration);
        _isDrillingForward = false;
        transform.Rotate(Vector3.up, -MeleeRotate);
        _drill.DrillStop();
        _meleeCollider.SetActive(false);
        _playerController.enabled = true;
        _playerShooterController.enabled = true;
        yield return new WaitForSeconds(MeleeColdDown);
        CanMelee = true;
    }
    private IEnumerator DrillingForward()
    {
        while (_isDrillingForward)
        {
            _controller.Move(Mathf.Lerp(DrillingForwardSpeed, 0, Time.deltaTime) * transform.forward);
            yield return null;
        }
    }
}
