using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;


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

    public string getName()
    {
        return ActionName;
    }

    public void doAction(Enemy e)
    {
        if (Type == "light")
        {
            e.restoreHealth(BasePower);
        }
        else if (Type == e.getType())
        {
            e.MagicAttack((int)(BasePower * 1.5));
        }
        else
        {
            e.MagicAttack(BasePower);
        }
    }
}


public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] private int maxHP = 50;
    private int currentHP = 1;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;
    [SerializeField] private string attackType = "";
    public List<EnemyAction> actions = new List<EnemyAction>();
    private Player player;
    [SerializeField] private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void setPlayer(Player p)
    {
        player = p;
    }
    public Vector3 getPosition()
    {
        return transform.position;
    }
    public void deathAnim()
    {
        anim.Play("Dead");
    }

    public void turnOff()
    {
    }
    public int getCurrentHP()
    {
        return currentHP;
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public float getDefense()
    {
        return baseDef;
    }
    public string getType()
    {
        return attackType;
    }
    public string getName()
    {
        return enemyName;
    }
    public void restoreHealth(int healthGained)
    {

        if (healthGained < 0)
        {
            anim.Play("Pain");
            currentHP += healthGained;
        }
        else if (currentHP == maxHP)
        {
            MagicAttack(10);
        }
        else
        {
            anim.Play("Heal");
            currentHP += healthGained;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }
    }
    public void MagicAttack(int baseDamage)
    {
        //modified version of pokemon damage calc
        anim.Play("Casting");
        double damage = (16 * baseDamage * (baseAtk / player.getDefense()) / 50) + 2;
        player.takeDamage((int)damage);
        return;
    }
    public void setAnimBool(string name, bool state)
    {
        anim.SetBool(name, state);
    }

    void Start()
    {
        currentHP = maxHP;
        attackType = attackType.ToLower();
        if (attackType == "fire")
        {
            actions.Add(new EnemyAction("Hot Moves", 30, "fire"));
        }
        else if (attackType == "wind")
        {
            actions.Add(new EnemyAction("Foul Wind", 30, "wind"));
        }
        actions.Add(new EnemyAction("Lesser Restoration", 30, "light"));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
