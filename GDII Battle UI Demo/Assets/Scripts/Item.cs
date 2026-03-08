using UnityEngine;

public class Item : MonoBehaviour
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
