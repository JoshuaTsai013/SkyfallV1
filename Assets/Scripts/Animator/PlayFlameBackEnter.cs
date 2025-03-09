using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayFlameBackEnter : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerManager.instance.FlameBackRight.Play();
        PlayerManager.instance.FlameBackLeft.Play();
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerManager.instance.FlameBackRight.Stop();
        PlayerManager.instance.FlameBackLeft.Stop();
    }
}
