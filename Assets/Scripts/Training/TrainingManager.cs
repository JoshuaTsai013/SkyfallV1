using System;
using System.Collections;
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
    public FadeOutTransitionScreen fadeOutTransitionScreen;
    private bool _sceneCanChange = true;

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

        if (ExitCollider.stay && _sceneCanChange)
        {
            _sceneCanChange = false;
            fadeOutTransitionScreen.FadeIn();
            StartCoroutine(LoadNextSceneWithDelay());
        }
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        // Load the next scene after a delay
        yield return new WaitForSeconds(3f); // Adjust the delay as needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
        gameObject.SetActive(false);
    }

    private void HandleTrainingStep(TrainingTrigger trigger, Image uiElement)
    {
        uiElement.gameObject.SetActive(trigger.stay);
    }
}



