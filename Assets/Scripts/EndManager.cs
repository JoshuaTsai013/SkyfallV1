using UnityEngine;

public class EndManager : MonoBehaviour
{

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void JumpToMainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void JunpToTrainingRoomScene()
    {
        // Load the training room scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void JumpToFightShipScene()
    {
        // Load the fight ship scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void JumpToOutDoorScene()
    {
        // Load the end scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

}
