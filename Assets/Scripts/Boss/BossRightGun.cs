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
        // Calculate the vector pointing from the gun barrel to the player
        Vector3 direction = PlayerManager.instance.player.transform.position - _gunTransform.position;

        // Draw a red debug line in the Scene view for troubleshooting
        Debug.DrawLine(_gunTransform.position, PlayerManager.instance.player.transform.position, Color.red);

        // Get the angle from the gun barrel to the player (Euler angle around the X-axis)
        float rawAngle = Quaternion.LookRotation(direction).eulerAngles.x;

        // Adjust the angle: Unity's Euler angles are in the range 0~360, convert to -180~180 for better readability
        if (rawAngle > 180f)
            rawAngle -= 360f;

        // Clamp the angle to the range -30° ~ 30° to prevent excessive rotation
        float clampedAngle = Mathf.Clamp(rawAngle, -30f, 30f);

        // Normalize the angle so that 0 represents pointing down and 1 represents pointing up
        float normalizedAngle = Mathf.InverseLerp(30f, -30f, clampedAngle);  // Adjust the range

        // Set the animation parameter
        _GunAngle = normalizedAngle;
        _animator.SetFloat("GunAngle", _GunAngle);
    }
}
