using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    public GameObject shootCam;
    public GameObject mainCam;
    public GameObject cross;
    public Animator _animator;
    public bool isAiming;
    private PlayerInputs Inputs;
    [SerializeField]
    private Gun _gun;
    private float _GunAngle;
    private Aimbot _aimbot;
    private void Start()
    {
        Inputs = GetComponent<PlayerInputs>();
        _aimbot = GetComponent<Aimbot>();
    }
    private void Update()
    {
        if (Inputs.aim)
        {
            isAiming = true;
            shootCam.SetActive(true);
            cross.SetActive(true);
            _animator.SetBool("Aim", true);
            GunAngle();
        }
        else
        {
            isAiming = false;
            shootCam.SetActive(false);
            cross.SetActive(false);
            _animator.SetBool("Aim", false);
            _animator.SetFloat("GunAngle", 0f);
            _aimbot.useAimAssist = false;
        }

        if (Inputs.shoot)
        {
            _gun.Shoot(Inputs.aim);
            _animator.SetBool("Shoot", true);
        }
        else
        {
            _animator.SetBool("Shoot", false);
        }

        if (isAiming && Inputs.isUsingController)
        {
            _aimbot.useAimAssist = true;
        }
    }

    private void GunAngle()
    {
        _GunAngle = mainCam.transform.eulerAngles.x;
        if (_GunAngle > 180)
        {
            _GunAngle -= 360;
        }

        _animator.SetFloat("GunAngle", _GunAngle);
    }
}
