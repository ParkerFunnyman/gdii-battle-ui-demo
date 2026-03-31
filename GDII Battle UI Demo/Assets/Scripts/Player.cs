using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;

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

    private AudioSource playerAS;
    [SerializeField] private AudioClip spellAudio;
    [SerializeField] private AudioClip meleeAudio;
    [SerializeField] private GameObject magicProjectile;

    public GameObject getProjectile()
    {
        return magicProjectile;
    }
    public void playAudios(string input)
    {
        input = input.ToLower();
        if (input == "melee")
        {
            playerAS.PlayOneShot(meleeAudio);
        }
        else if (input == "spell")
        {
            playerAS.PlayOneShot(spellAudio);
        }
    }

    //plays ONLY spell audio with delay
    public void playAudios(float delay)
    {
        playerAS.PlayDelayed(delay);
    }
    public int getCurrentHP()
    {
        return currentHP;
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public int getCurrentMana()
    {
        return currentMana;
    }

    public int getMaxMana()
    {
        return maxMana;
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
            currentMana -= manaUse;
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
        playerAS = GetComponent<AudioSource>();

        playerSpells.Add(new Spell("Staff Attack", 40, "melee", 0));
        playerSpells.Add(new Spell("Fireball", 60, "fire", 10));
        playerSpells.Add(new Spell("Wind Blast", 60, "wind", 10));
        playerSpells.Add(new Spell("Lesser Restoration", 35, "light", 5));
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
