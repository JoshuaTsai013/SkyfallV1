using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float volume = 0.1f;
    private void PlayFootstepSound()
    {
        SoundManager.PlaySound(SoundType.Footstep, volume);
        InvokeRepeating("PlayFootstepSound", 0, 0.5f);
    }
}
