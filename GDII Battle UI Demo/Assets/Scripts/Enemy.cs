using UnityEngine;
using System.Collections.Generic;
using System;


public class EnemyAction
{
    private string ActionName;
    private int BasePower;
    private string Type;

    public EnemyAction(string actionName, int basePower, string type)
    {
        ActionName = actionName;
        BasePower = basePower;
        Type = type.ToLower();
    }

    public string getType()
    {
        return Type;
    }

    public int getPower()
    {
        return BasePower;
    }

    public void doAction(Enemy e)
    {
        if (Type == "heal")
        {
            e.restoreHealth(BasePower);
        }
        else
        {
            e.MagicAttack(BasePower);
        }
    }
}


public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHP = 50;
    private int currentHP = 50;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;
    [SerializeField] private string attackType = "";
    public List<EnemyAction> actions = new List<EnemyAction>();
    public Player player;
    [SerializeField] private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public int getCurrentHP()
    {
        return currentHP;
    }
    public float getDefense()
    {
        return baseDef;
    }

    public void restoreHealth(int healthGained)
    {
        anim.Play("Backflip");
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        
        if (currentHP == maxHP)
        {
            MagicAttack(10);
        }
        else
        {
            currentHP += healthGained;
        }
    }
    public void MagicAttack(int baseDamage)
    {
        //modified version of pokemon damage calc
        anim.Play("Casting");
        double damage = (((16 * baseDamage * (baseAtk / player.getDefense()))/50) + 2) * (UnityEngine.Random.Range(85, 101) / 100);
        player.takeDamage((int)damage);
        return;
    }
    public void setAnimBool(string name, bool state)
    {
        anim.SetBool(name, state);
    }

    void Start()
    {
        anim.SetBool("attack", false);
        anim.SetBool("healing", false);
        player.enemiesInScene.Add(this);
        attackType = attackType.ToLower();
        actions.Add(new EnemyAction("Hot Moves", 30, "fire"));
        actions.Add(new EnemyAction("Lesser Restoration", 30, "heal"));
        actions[1].doAction(this);
    }

    // Update is called once per frame
    void Update()
    {
        //actions[0].doAction(this);
    }
}
