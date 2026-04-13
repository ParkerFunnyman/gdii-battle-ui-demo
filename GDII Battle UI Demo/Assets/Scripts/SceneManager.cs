using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
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

    public bool is_field;

    void Start()
    {
        items.Add(new Item("The EVIL Eye of John Enemy"));
    }
}
