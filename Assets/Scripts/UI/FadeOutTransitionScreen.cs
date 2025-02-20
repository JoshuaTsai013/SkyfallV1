  using UnityEngine;
using DG.Tweening;

public class FadeOutTransitionScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _TransitionScreen;
    [SerializeField] private float _fadeDuration = 1f;
    private void Start()
    {
      _TransitionScreen.DOFade(0, _fadeDuration).SetEase(Ease.Linear);
    }
}
