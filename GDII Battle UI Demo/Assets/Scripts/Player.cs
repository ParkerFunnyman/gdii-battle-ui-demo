using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class Spell
{
    private string SpellName;
    private int BasePower;
    private string SpellType;

    public Spell(string spellName, int basePower, string spellType)
    {
        SpellName = spellName;
        BasePower = basePower;
        SpellType = spellType.ToLower();
    }

    public string getSpellName()
    {
        return SpellName;
    }

    public void castSpell(Player p, Enemy e)
    {
        if (SpellType == "light"){
            p.restoreHealth(BasePower);
        }
        else
        {
            if (SpellType == "melee")
            {
                p.playAnim("Slashing");
            }
            else
            {
                p.playAnim("Casting");
            }
            double damage = ((16 * BasePower * ( p.getAttack()/ e.getDefense())/50) + 2) * (UnityEngine.Random.Range(85, 101) / 100);
            e.restoreHealth(-(int)damage);
        }
    }
}

public class Item
{
    private string ItemName;
    public Item(string name)
    {
        ItemName = name;
    }
}

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHP = 120;
    private int currentHP = 120;
    [SerializeField] private int maxMana = 50;
    private int currentMana = 50;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;

    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI manaText;

    public List<Spell> playerSpells = new List<Spell>();
    public List<Item> playerItems = new List<Item>();

    public int getCurrentHP()
    {
        return currentHP;
    }
    public float getDefense()
    {
        return baseDef;
    }

    public float getAttack()
    {
        return baseAtk;
    }
    public void takeDamage(int damageDealt)
    {
        currentHP -= damageDealt;
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

    public void playAnim(string animToDo)
    {
        anim.Play(animToDo);
        return;
    }

    public void die()
    {
        anim.Play("Dead");
        //insert function to end battle, return to last save, delete player's system32, etc.
        return;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSpells.Add(new Spell("Staff Attack", 40, "melee"));
        playerSpells.Add(new Spell("Fireball", 80, "fire"));
        playerSpells.Add(new Spell("Wind Blast", 80, "wind"));
        playerSpells.Add(new Spell("Minor Restoration", 40, "light"));
    }

    // Update is called once per frame
    void Update()
    {
        //Keeps HP values in reasonable range
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        else if (currentHP < 0)
        {
            currentHP = 0;
        }

        //Keeps Mana values in reasonable range
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        else if (currentMana < 0)
        {
            currentMana = 0;
        }

        //Updates UI text
        HPText.text = "HP: " + currentHP.ToString() + " / " + maxHP.ToString();
        manaText.text = "Mana: " + currentMana.ToString() + " / " + maxMana.ToString();
    }
}
