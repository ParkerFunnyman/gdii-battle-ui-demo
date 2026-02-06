using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private int maxHP = 50;
    private int currentHP = 50;
    private int maxMana = 50;
    private int currentMana = 50;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;

    public TextMeshProUGUI HPText;

    public void dealDamage(int damageDealt)
    {
        currentHP -= damageDealt;
        if (currentHP <= 0)
        {
            die();
        }
    }

    public void restoreHealth(int healthGained)
    {
        currentHP += healthGained;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public bool useMana(int manaUse)
    {
        if ((currentMana - manaUse) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void restoreMana(int manaGain)
    {
        currentMana += manaGain;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    public void die()
    {
        return; //DO LATER
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPText.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        HPText.text = currentHP.ToString() + " / " + maxHP.ToString();
    }
}
