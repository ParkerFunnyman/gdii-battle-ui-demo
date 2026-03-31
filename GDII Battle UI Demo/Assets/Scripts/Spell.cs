using Unity.VisualScripting;
using UnityEngine;

public class Spell
{
    private string SpellName;
    private int BasePower;
    private string SpellType;
    private int ManaCost;

    public Spell(string spellName, int basePower, string spellType, int manaCost)
    {
        SpellName = spellName;
        BasePower = basePower;
        SpellType = spellType.ToLower();
        ManaCost = manaCost;
    }

    public string getSpellName()
    {
        return SpellName;
    }

    public int getManaCost()
    {
        return ManaCost;
    }

    public string getSpellType()
    {
        return SpellType;
    }

    public void castSpell(Player p, Enemy e)
    {
        Transform playerT = p.GetComponent<Transform>();
        Transform enemyT = e.GetComponent<Transform>();
        Vector3 direction = (enemyT.position - playerT.position).normalized;
        if (direction != Vector3.zero) // Avoid errors if positions are identical
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            playerT.rotation = lookRotation;
        }
        if (SpellType == "melee")
        {
            p.playAudios("melee");
            p.playAnim("Slashing");
            double damage = (16 * BasePower * (p.getAttack() / e.getDefense()) / 50.0) + 2;
            //Debug.Log(damage + "  " + (-(int)damage) + "  " + e.getCurrentHP());
            e.restoreHealth(-(int)damage);
        }
        else
        {
            if (p.useMana(ManaCost))
            {
                
                p.playAudios(0.5f);
                if (SpellType == "light")
                {
                    p.playAnim("Healing");
                    p.restoreHealth(BasePower);
                }
                else
                {
                    p.playAnim("Casting");
                    //summon magic ball
                    double damage = (16 * BasePower * (p.getAttack() / e.getDefense()) / 50.0) + 2;
                    //Debug.Log(damage + "  " + (-(int)damage) + "  " + e.getCurrentHP());
                    e.restoreHealth(-(int)damage);
                }
            }
            else
            {
                //Debug.Log("sadge");
            }
        }
    }
}
