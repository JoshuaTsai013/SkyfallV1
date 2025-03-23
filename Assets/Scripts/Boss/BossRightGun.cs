using UnityEngine;

public class BossRightGun : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _GunAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GunAngle();
    }

    private void GunAngle()
    {
        Vector3 direction = PlayerManager.instance.player.transform.position - transform.position;
        float targetAngle = Quaternion.LookRotation(direction).eulerAngles.x;
        targetAngle = Mathf.Clamp(targetAngle, 0, 90);
        // _GunAngle = targetAngle;
        _GunAngle = Mathf.LerpAngle(_GunAngle, targetAngle, Time.deltaTime * 2f);
        

        _animator.SetFloat("GunAngle", _GunAngle);
    }
}
