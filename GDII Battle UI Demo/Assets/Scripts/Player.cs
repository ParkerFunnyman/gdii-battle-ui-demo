using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Spell
{
    private string SpellName;
    private int BasePower;

    public Spell(string spellName, int basePower)
    {
        SpellName = spellName;
        BasePower = basePower;
    }

    public string getSpellName()
    {
        return SpellName;
    }
}

public class Item
{
    public string ItemName;
}

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHP = 50;
    private int currentHP = 50;
    [SerializeField] private int maxMana = 50;
    private int currentMana = 50;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;

    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI manaText;

    private List<Spell> playerSpells = new List<Spell>();
    public List<Enemy> enemiesInScene = new List<Enemy>();

    public void takeDamage(int damageDealt)
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
        currentHP = maxHP;
        currentMana = maxMana;

        playerSpells.Add(new Spell("fireball", 80));
    }

    // Update is called once per frame
    void Update()
    {
        HPText.text = "HP: " + currentHP.ToString() + " / " + maxHP.ToString();
        manaText.text = "Mana: " + currentMana.ToString() + " / " + maxMana.ToString();
    }
}
