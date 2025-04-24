using UnityEngine;
using UnityEngine.VFX;

public class TankTrackDustVFXManager : MonoBehaviour
{
    public VisualEffect[] vfxArray;

    public void TriggerVFX(string eventName)
    {
        if (vfxArray == null || vfxArray.Length == 0)
        {
            Debug.LogWarning("No Visual Effects assigned in PlayVFXState.");
            return;
        }
        foreach (VisualEffect effect in vfxArray)
        {
            if (effect != null)
            {
                effect.SendEvent(eventName);
            }
        }
        }
}
