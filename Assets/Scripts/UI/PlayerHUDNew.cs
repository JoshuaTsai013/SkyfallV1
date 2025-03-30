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
  public int AmmoAmount;
  public int RepairAmount;
  GameObject player;
  CharacterGeneral characterGeneral;
  Heat heat;
  private void Start()
  {
    player = PlayerManager.instance.player;
    characterGeneral = player.GetComponent<CharacterGeneral>();
    heat = player.GetComponent<Heat>();

    BloodBar.fillAmount = 1;
    SetHeatBar(0);
    SetBloodText(1);
  }
  private void FixedUpdate()
  {
    SetHeatBar(heat.HeatPercentage);
    BloodBar.fillAmount = characterGeneral.HealthPercentage;
    SetBloodText(characterGeneral.HealthPercentage);
    SetAmmoText(AmmoAmount);
    SetRepairText(RepairAmount);
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
