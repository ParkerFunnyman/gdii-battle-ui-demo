using UnityEngine;

public class Spell : MonoBehaviour
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

    public void castSpell(Player p, Enemy e)
    {
        if (SpellType == "melee")
        {
            p.playAnim("Slashing");
            double damage = (16 * BasePower * ( p.getAttack()/ e.getDefense())/50.0) + 2;
            //Debug.Log(damage + "  " + (-(int)damage) + "  " + e.getCurrentHP());
            e.restoreHealth(-(int)damage);
        }
        else{
            if (p.useMana(ManaCost)){
            
                p.playAnim("Casting");
                if (SpellType == "light")
                {
                    p.restoreHealth(BasePower);
                }
                else
                {
                    double damage = (16 * BasePower * ( p.getAttack()/ e.getDefense())/50.0) + 2;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
