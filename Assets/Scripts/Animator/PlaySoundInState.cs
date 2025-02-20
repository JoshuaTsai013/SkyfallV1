using UnityEngine;

public class PlaySoundInState : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1;
    private float timer = 0;
    [SerializeField] private float interval = 0.5f;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            SoundManager.PlaySound(sound, volume);
            timer = 0;
        }
    }
}
