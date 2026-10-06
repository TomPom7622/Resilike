using TMPro;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TMP_Text healthBar;

    private string healthBarText = "/100";

    private void Awake()
    {
        healthBar = GetComponent<TMP_Text>();
    }

    public void UpdateHealth(int health)
    {
        healthBar.text=health.ToString()+healthBarText;
    }

    public void TurnHealthBarOnOrOff()
    {
        if (healthBar.enabled)
            healthBar.enabled=false;
        else
            healthBar.enabled=true;
    }
    public void TurnHealthBarOn()
    { 
        healthBar.enabled = true; 
    }
    public void TurnHealthBarOf()
    { 
        healthBar.enabled = false; 
    }
}
