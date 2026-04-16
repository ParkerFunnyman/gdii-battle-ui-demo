using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Diagnostics.CodeAnalysis;

public class Player : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private int maxHP = 120;
    private int currentHP = 120;
    [SerializeField] private int maxMana = 50;
    private int currentMana = 50;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;

    [Header("Player Inventory")]
    public List<Spell> playerSpells = new List<Spell>();
    public List<Item> playerItems = SceneManager.items;

    [Header("Player GameObjects")]
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI manaText;
    private AudioSource playerAS;
    [SerializeField] private AudioClip spellAudio;
    [SerializeField] private AudioClip meleeAudio;
    [SerializeField] private GameObject magicProjectile;

    [Header("Spell Materials")]
    [SerializeField] private Material fire;
    [SerializeField] private Material fireParticle;
    [SerializeField] private Material ice;
    [SerializeField] private Material iceParticle;
    [SerializeField] private Material wind;
    [SerializeField] private Material windParticle;
    [SerializeField] private Material thunder;
    [SerializeField] private Material thunderParticle;
    [SerializeField] private Material earth;
    [SerializeField] private Material earthParticle;


    public void fireSpell(Enemy e, string type)
    {
        StartCoroutine(wait(0.5f, type, e));

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
        StartCoroutine(HurtAnim(1.0f));
        currentHP -= damageDealt;
    }
    IEnumerator HurtAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.Play("Hurt");
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
        Transform playerT = GetComponent<Transform>();
        playerT.position = new Vector3(playerT.position.x, 0.5f, -7.5f);
        anim.Play("Dead");
        //insert function to end battle, return to last save, delete player's system32, etc.
        return;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAS = GetComponent<AudioSource>();
        playerSpells.Add(new Spell("Staff Attack", 40, "melee", 0));
        playerSpells.AddRange(SceneManager.spells);
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

    IEnumerator wait(float time, string type, Enemy e)
    {
        yield return new WaitForSeconds(time);
        GameObject spellBall = Instantiate(magicProjectile);
        Renderer rend = spellBall.GetComponent<Renderer>();
        ParticleSystemRenderer PSrend = spellBall.GetComponent<ParticleSystemRenderer>();
        Transform enemyT = e.GetComponent<Transform>();
        Transform playerT = GetComponent<Transform>();
        spellBall.transform.position = new Vector3(playerT.position.x, playerT.position.y + 0.67f, playerT.position.z);
        if (type == "fire")
        {
            rend.material = fire;
            PSrend.material = fireParticle;
        }
        else if (type == "ice")
        {
            rend.material = ice;
            PSrend.material = iceParticle;
        }
        else if (type == "wind")
        {
            rend.material.color = Color.seaGreen;
            PSrend.material = windParticle;
        }
        else if (type == "thunder")
        {
            rend.material.color = Color.yellow;
            PSrend.material = thunderParticle;
        }
        else if (type == "earth")
        {
            rend.material = earth;
            PSrend.material = earthParticle;
        }
        else
        {
            rend.material.color = Color.black;
        }
        Vector3 direction = (enemyT.position - playerT.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, -0.13f, direction.z));
            spellBall.transform.rotation = lookRotation;
        }
        yield return new WaitForSeconds(1);
        Destroy(spellBall );
    }
}



