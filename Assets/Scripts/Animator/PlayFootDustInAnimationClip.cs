using UnityEngine;

public class PlayFootDustInAnimationClip : MonoBehaviour
{
    [SerializeField] private ParticleSystem _footDustLeft;
    [SerializeField] private ParticleSystem _footDustRight;
    private void PlayFootDustLeft()
    {
        _footDustLeft.Play();
    }
    private void PlayFootDustRight()
    {
        _footDustRight.Play();
    }
}
