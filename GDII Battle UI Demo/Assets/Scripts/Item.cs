using UnityEngine;

public class Item
{
    private string ItemName;
    private string HealTarget;
    private int value;
    public Item(string name, string target, int v)
    {
        ItemName = name;
        HealTarget = target.ToLower();
        value = v;
    }

    public string getItemName()
    {
        return ItemName;
    }

    public void useItem(Player p)
    {
        if (HealTarget == "hp")
        {
            p.restoreHealth(value);
        }
        else if (HealTarget == "mana"){
            p.restoreMana(value);
        }
        p.playAnim("Item");
    }

}
