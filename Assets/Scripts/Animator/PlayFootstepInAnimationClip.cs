using UnityEngine;

public class PlayFootstepInAnimationClip : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float volume = 0.1f;
    private void PlayFootstepInAnimation()
    {
        SoundManager.PlaySound(SoundType.Footstep, volume);
    }
}
