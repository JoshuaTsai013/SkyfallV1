using UnityEngine;

public class BossRightGun : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _GunAngle = 0f;
    [SerializeField]
    private Transform _gunTransform;
    // Update is called once per frame
    void Update()
    {
        GunAngle();
        // if (Time.frameCount % 120 == 0)
        // {
        //     _animator.SetTrigger("RightGunShot");
        // }
    }

    private void GunAngle()
    {
        Vector3 direction = PlayerManager.instance.player.transform.position - _gunTransform.position;
        Debug.DrawLine(_gunTransform.position, PlayerManager.instance.player.transform.position, Color.red);
        float targetAngle = Quaternion.LookRotation(direction).eulerAngles.x;
        // targetAngle = Mathf.Clamp(targetAngle, 0, 90);
        _GunAngle = targetAngle;
        // _GunAngle = Mathf.LerpAngle(_GunAngle, targetAngle, Time.deltaTime * 2f);
        

        _animator.SetFloat("GunAngle", _GunAngle);
    }
}
