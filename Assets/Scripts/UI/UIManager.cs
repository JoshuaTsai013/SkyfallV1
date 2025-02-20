using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerHUD hud;
    GameObject player;
    CharacterGeneral characterGeneral;
    Heat heat;
    private void Start()
    {
        player = PlayerManager.instance.player;
        characterGeneral = player.GetComponent<CharacterGeneral>();
        heat = player.GetComponent<Heat>();
        hud.SetBloodBar(1);
        hud.SetHeatBar(0);
    }
    private void FixedUpdate()
    {
        hud.SetBloodBar(characterGeneral.HealthPercentage);
        hud.SetHeatBar(heat.HeatPercentage);
    }
}
