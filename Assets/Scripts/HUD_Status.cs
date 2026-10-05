using UnityEngine;
using UnityEngine.UI;
using TMPro;

//HUD gore lijevo: health, stamina i ruple
public class HUD_Status : MonoBehaviour
{
    public Image HealthFill;
    public TextMeshProUGUI HealthText;
    public Image StaminaFill;
    public TextMeshProUGUI StaminaText;
    public TextMeshProUGUI RupleText;

    void Update()
    {
        HealthFill.fillAmount = (float)Player_Controler.CurrentHealth / Player_Controler.MaxHealth;
        HealthText.text = Player_Controler.CurrentHealth + " / " + Player_Controler.MaxHealth;
        StaminaFill.fillAmount = Player_Controler.CurrentStamina / Player_Controler.MaxStamina;
        StaminaText.text = (int)Player_Controler.CurrentStamina + " / " + Player_Controler.MaxStamina;
        RupleText.text = Player_Controler.RupleCurrency.ToString();
    }
}
