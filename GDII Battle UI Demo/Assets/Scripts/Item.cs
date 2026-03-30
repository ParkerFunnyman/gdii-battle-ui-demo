using UnityEngine;

public class Item
{
    private string ItemName;
    public Item(string name)
    {
        ItemName = name;
    }

    private string getItemName()
    {
        return ItemName;
    }

}
