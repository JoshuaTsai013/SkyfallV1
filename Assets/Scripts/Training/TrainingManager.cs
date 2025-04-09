using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TrainingTrigger ExitCollider;

    public TrainingDoorOpen door;
    void Start()
    {
        Jump.gameObject.SetActive(false);
        Dash.gameObject.SetActive(false);
        Shoot.gameObject.SetActive(false);
        Melee.gameObject.SetActive(false);
        Overheat.gameObject.SetActive(false);
    }
    void Update()
    {
        HandleTrainingStep(MoveCollider, Move);
        HandleTrainingStep(JumpCollider, Jump);
        HandleTrainingStep(DashCollider, Dash);
        HandleTrainingStep(ShootCollider, Shoot);
        HandleTrainingStep(MeleeCollider, Melee);
        HandleTrainingStep(OverheatCollider, Overheat);

        if (NextCollider.stay)
        {
            door.OpenDoor();
            NextCollider.stay = false;
            NextCollider.gameObject.SetActive(false);
        }

        if (ExitCollider.stay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            gameObject.SetActive(false);
        }
    }

    private void HandleTrainingStep(TrainingTrigger trigger, Image uiElement)
    {
        uiElement.gameObject.SetActive(trigger.stay);
    }
}



