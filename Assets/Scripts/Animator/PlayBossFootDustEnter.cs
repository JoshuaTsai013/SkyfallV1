using UnityEngine;

public class PlayBossFootDustEnter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent<TankTrackDustVFXManager>(out var manager))
        {
            manager.TriggerVFX("TankPlay");
        }
        else
        {
            Debug.LogWarning("TankTrackDustVFXManager component not found on the animator.");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent<TankTrackDustVFXManager>(out var manager))
        {
            manager.TriggerVFX("TankStop");
        }
        else
        {
            Debug.LogWarning("TankTrackDustVFXManager component not found on the animator.");
        }
    }
}

