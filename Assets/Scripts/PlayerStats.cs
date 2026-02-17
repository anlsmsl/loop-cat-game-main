using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public int maxHunger = 3;
    public int currentHunger;


    public UIHandler uiHandler;

    private void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        uiHandler.UpdateUI(currentHealth, currentHunger);
    }

    public void ChangeHunger(int amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
        uiHandler.UpdateUI(currentHealth, currentHunger);
    }

    
}
