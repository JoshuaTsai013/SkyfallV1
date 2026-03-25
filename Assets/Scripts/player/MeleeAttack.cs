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
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Drill _drill;

    public bool CanMelee;
    public bool IsDrilling = false;
    private bool _isDrillingForward;

    private CharacterController _controller;

    private ThirdPersonShooterController _playerShooterController;
    [SerializeField]
    private CinemachineImpulseSource _impulseSource;
    [SerializeField]
    private GameObject _meleeCollider;
    private Aimbot _aimbot;
    private float _rotationVelocity;
    private float _targetRotation;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerShooterController = GetComponent<ThirdPersonShooterController>();
        _aimbot = GetComponent<Aimbot>();
        _meleeCollider.SetActive(false);
        CanMelee = true;
    }
    public void TryMeleeAttack()
    {
        if (CanMelee)
        {
            IsDrilling = true;
            CanMelee = false;
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
        _aimbot.useAimAssist = true;
        Debug.Log("Melee Attack!!");
        yield return new WaitForSeconds(ChargeDuration);
        //drillingForward
        _meleeCollider.SetActive(true);

        _isDrillingForward = true;
        StartCoroutine(DrillingForward());
        meleeCam.SetActive(false);
        yield return new WaitForSeconds(DrillDuration);
        _isDrillingForward = false;
        _drill.DrillStop();
        _aimbot.useAimAssist = false;
        IsDrilling = false;
        _playerShooterController.enabled = true;
        yield return new WaitForSeconds(MeleeColdDown);
        CanMelee = true;
    }
    private IEnumerator DrillingForward()
    {
        _targetRotation = _aimbot.mainCamera.transform.eulerAngles.y;
        while (_isDrillingForward)
        {
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    0.1f);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            _controller.Move(Mathf.Lerp(DrillingForwardSpeed, 0, Time.deltaTime) * transform.forward);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        _meleeCollider.SetActive(false);
    }
}
