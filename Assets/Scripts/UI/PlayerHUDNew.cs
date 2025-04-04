using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDNew : MonoBehaviour
{
  public Image HeatBarL;
  public Image HeatBarR;
  public Image BloodBar;
  public TextMeshProUGUI HealthText;
  public TextMeshProUGUI AmmoText;
  public TextMeshProUGUI RepairText;
  PlayerStats _playerStats;
  private void Start()
  {
    _playerStats = PlayerManager.instance.playerStats;
    BloodBar.fillAmount = 1;
    SetHeatBar(0);
    SetBloodText(1);
  }
  private void FixedUpdate()
  {
    SetHeatBar(_playerStats.HeatPercentage);
    BloodBar.fillAmount = _playerStats.HealthPercentage;
    SetBloodText(_playerStats.HealthPercentage);
    SetAmmoText(_playerStats.AmmoAmount);
    SetRepairText(_playerStats.RepairAmount);
  }
  private void SetHeatBar(float value)
  {
    HeatBarL.fillAmount = value;
    HeatBarR.fillAmount = value;
  }
  private void SetBloodText(float value)
  {
    value *= 100f;
    string text = value.ToString("00") + "%";
    HealthText.SetText(text);
  }
  public void SetAmmoText(int value)
  {
    string text = value.ToString("00");
    AmmoText.SetText(text);
  }
  public void SetRepairText(int value)
  {
    string text = value.ToString("00");
    RepairText.SetText(text);
  }
}
