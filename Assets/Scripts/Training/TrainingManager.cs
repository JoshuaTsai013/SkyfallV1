using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : MonoBehaviour
{
    public Image Move;
    public Image Jump;
    public Image Dash;
    public Image Shoot;
    public Image Exit;

    public TrainingTrigger MoveCollider;
    public TrainingTrigger JumpCollider;
    public TrainingTrigger NextCollider;
    public TrainingTrigger DashCollider;
    public TrainingTrigger ShootCollider;
    public TrainingTrigger ExitCollider;

    public TrainingDoorOpen door;
    void Start()
    {
        Jump.gameObject.SetActive(false);
        Dash.gameObject.SetActive(false);
        Shoot.gameObject.SetActive(false);
        Exit.gameObject.SetActive(false);
        // PlayerManager.instance.player.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
    }
    void Update()
    {
        if (MoveCollider.stay)
        {
            ShowMove();
        }
        else
        {
            HideMove();
        }

        if (JumpCollider.stay)
        {
            HideMove();
            ShowJump();
        }
        else
        {
            HideJump();
        }

        if (DashCollider.stay)
        {
            HideJump();
            ShowDash();
        }
        else
        {
            HideDash();
        }

        if (ShootCollider.stay)
        {
            HideDash();
            ShowShoot();
        }
        else
        {
            HideShoot();
        }

        if (NextCollider.stay)
        {
            door.OpenDoor();
            NextCollider.stay = false;
            NextCollider.gameObject.SetActive(false);
        }

        if (ExitCollider.stay)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            gameObject.SetActive(false);
        }
    }
    private void ShowMove()
    {
        Move.gameObject.SetActive(true);
    }

    private void ShowJump()
    {
        Jump.gameObject.SetActive(true);
    }

    private void ShowDash()
    {
        Dash.gameObject.SetActive(true);
    }

    private void ShowShoot()
    {
        Shoot.gameObject.SetActive(true);
    }

    private void HideMove()
    {
        Move.gameObject.SetActive(false);
    }

    private void HideJump()
    {
        Jump.gameObject.SetActive(false);
    }

    private void HideDash()
    {
        Dash.gameObject.SetActive(false);
    }

    private void HideShoot()
    {
        Shoot.gameObject.SetActive(false);
    }
}



