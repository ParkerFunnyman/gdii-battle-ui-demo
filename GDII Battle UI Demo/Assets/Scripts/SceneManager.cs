using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static int totalXP = 0;
    public static List<Spell> spells = new List<Spell>{
        new Spell("Fireball", 60, "fire", 10),
        new Spell("Glacial Freeze", 60, "ice", 10), 
        new Spell("Mach Wave", 60, "wind", 10),
        new Spell("Induction", 60, "thunder", 10), 
        new Spell("Brickwork", 60, "earth", 10),
        new Spell("Lesser Restoration", 35, "light", 5), 
        new Spell("Mana Drain", 10, "dark", -5)
    };
    public static List<Item> items = new List<Item>();

    void Start()
    {
        Item potion = new Item("Potion", "hp", 25);
        Item manaRestore = new Item("Mana Restore", "mana", 15);

        for (int i = 0; i < 3; i++)
        {
            items.Add(potion);
        }
        for (int i = 0; i < 2; i++)
        {
            items.Add(manaRestore);
        }


    }
}
