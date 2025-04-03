using UnityEngine;
using UnityEngine.VFX;

public class PlayerManager : MonoBehaviour
{
  #region Singleton
  public static PlayerManager instance;
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
      Destroy(gameObject);
  }
  #endregion
  public PlayerStats playerStats;
  public GameObject player;
  public GameObject PlayerCamera;
  public VisualEffect FlameBackRight;
  public VisualEffect FlameBackLeft;
  public VisualEffect FlameSideRight;
  public VisualEffect FlameSideLeft;
  public VisualEffect FlameDrill;

}