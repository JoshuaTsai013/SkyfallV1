using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : MonoBehaviour
{
    public Image Move;
    public Image Jump;
    public Image Dash;
    public Image Shoot;
    public Image Melee;
    public Image Overheat;

    public TrainingTrigger MoveCollider;
    public TrainingTrigger JumpCollider;
    public TrainingTrigger NextCollider;
    public TrainingTrigger DashCollider;
    public TrainingTrigger ShootCollider;
    public TrainingTrigger MeleeCollider;
    public TrainingTrigger OverheatCollider;
    
    public FadeOutTransitionScreen fadeOutTransitionScreen;

    void Start()
    {
        Jump.gameObject.SetActive(false);
        Dash.gameObject.SetActive(false);
        Shoot.gameObject.SetActive(false);
        Melee.gameObject.SetActive(false);
        Overheat.gameObject.SetActive(false);

        BindTrainingStep(MoveCollider, Move);
        BindTrainingStep(JumpCollider, Jump);
        BindTrainingStep(DashCollider, Dash);
        BindTrainingStep(ShootCollider, Shoot);
        BindTrainingStep(MeleeCollider, Melee);
        BindTrainingStep(OverheatCollider, Overheat);
    }

    private void BindTrainingStep(TrainingTrigger trigger, Image uiElement)
    {
        if (trigger != null && uiElement != null)
        {
            trigger.onTriggerEnterEvent.AddListener(() => uiElement.gameObject.SetActive(true));
            trigger.onTriggerExitEvent.AddListener(() => uiElement.gameObject.SetActive(false));
        }
    }
}
