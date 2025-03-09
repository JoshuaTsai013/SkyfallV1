using UnityEngine;
using UnityEngine.VFX;

public class PlayerManager : MonoBehaviour
{
  #region Singleton
   public static PlayerManager instance;
    void  Awake()
    {
        instance = this;
    }
  #endregion

  public GameObject player;
  public GameObject PlayerCamera;
  public VisualEffect FlameBackRight;
  public VisualEffect FlameBackLeft;
  public VisualEffect FlameSideRight;
  public VisualEffect FlameSideLeft;
  public VisualEffect FlameDrill;

}