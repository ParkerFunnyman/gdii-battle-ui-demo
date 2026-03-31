using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public Slider healthBar;
    public Slider manaBar;

    public void SetMaxHealth(int maxHealth, int maxMana)
    {
        healthBar.maxValue = maxHealth;
        manaBar.maxValue = maxMana;

        healthBar.value = maxHealth;
        manaBar.value = maxMana;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }

    public void SetMana(int mana)
    {
        manaBar.value = mana;
    }
}
