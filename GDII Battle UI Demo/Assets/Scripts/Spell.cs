using UnityEngine;

public class Spell : MonoBehaviour
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
            double damage = (16 * BasePower * ( p.getAttack()/ e.getDefense())/50.0) + 2;
            //Debug.Log(damage + "  " + (-(int)damage) + "  " + e.getCurrentHP());
            e.restoreHealth(-(int)damage);
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
