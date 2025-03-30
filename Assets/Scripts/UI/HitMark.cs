using TMPro;
using UnityEngine;
using DG.Tweening;

public class HitMark : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _hitMarkText;
    [SerializeField] private CharacterGeneral characterGeneral;
    private void Awake()
    {
        if (characterGeneral)
        {
            characterGeneral.OnHit.AddListener(ShowHitMark);
        }
        _hitMarkText.CrossFadeAlpha(0, 0f, true);
    }
    public void ShowHitMark()
    {
        _hitMarkText.SetText(characterGeneral.damageLastTime.ToString());
        _hitMarkText.CrossFadeAlpha(1, 0.1f, false);
        _hitMarkText.transform.DOScale(1.5f, 0.1f).OnComplete(() =>
        {
            _hitMarkText.CrossFadeAlpha(0, 0.3f, false);
            _hitMarkText.transform.DOScale(1, 0.5f);
        });
    }
    private void OnDisable()
    {
        if (characterGeneral)
        {
            characterGeneral.OnHit.AddListener(ShowHitMark);
        }
    }
}
