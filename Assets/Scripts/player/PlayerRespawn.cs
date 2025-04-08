using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn Point")]// The current spawn point for the player. This is set by the Checkpoint script when the player reaches a checkpoint.
    private static Transform currentSpawnPoint;

    [Header("UI Inital State")]//set all to active false;
    [SerializeField] private GameObject _PauseMenuUI;
    [SerializeField] private GameObject _GameOverUI;

    [Header("Player State")]
    [SerializeField] private GameObject _mechModel;

    private void Awake()
    {
        if (currentSpawnPoint == null)
        {
            currentSpawnPoint = transform;
        }
    }

    // Called by Checkpoint
    public static void SetSpawnPoint(Transform newSpawn)
    {
        currentSpawnPoint = newSpawn;
    }

    public void Respawn()
    {
        if (currentSpawnPoint == null)
        {
            Debug.LogError("No spawn point set!");
            return;
        }
        // Deactivate UI elements

        _PauseMenuUI.SetActive(false);
        _GameOverUI.SetActive(false);

        // enable player controls
        _mechModel.SetActive(true);
        gameObject.GetComponent<ThirdPersonController>().enabled = true;
        gameObject.GetComponent<ThirdPersonShooterController>().enabled = true;
        gameObject.GetComponent<MeleeAttack>().enabled = true;
        gameObject.GetComponent<PlayerInput>().enabled = true;
        

        // Move player
        transform.SetPositionAndRotation(currentSpawnPoint.position, currentSpawnPoint.rotation);
    }
}
