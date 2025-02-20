using UnityEngine;

public class PlayParticleInState : StateMachineBehaviour
{
    [SerializeField] private ParticleSystem particle;
    private float timer = 0;
    [SerializeField] private float interval = 0.5f;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            Instantiate(particle, animator.transform.position, Quaternion.identity);
            timer = 0;
        }
    }
}
