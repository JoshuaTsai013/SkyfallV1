using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedCrosshair : MonoBehaviour
{
    public Image CrosshairImage;
    private void Start()
    {
        CrosshairImage.transform.localScale = Vector3.one * 0.4f;
    }

    public void AnimatedCrosshairFire()
    {
        CrosshairImage.transform.DOScale(0.7f, 0.1f).OnComplete(() =>
         {
             CrosshairImage.transform.DOScale(0.4f, 0.3f);
         });
    }

}
