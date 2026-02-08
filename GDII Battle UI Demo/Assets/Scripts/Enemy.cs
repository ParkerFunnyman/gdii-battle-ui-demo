using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHP = 50;
    private int currentHP = 50;
    private float baseAtk = 50.0f;
    private float baseDef = 50.0f;
    [SerializeField] private string attackType = "";
    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public int getCurrentHP()
    {
        return currentHP;
    }
    public float getDefense()
    {
        return baseDef;
    }
    void MagicAttack(int baseDamage)
    {
        //modified version of pokemon damage calc
        double damage = (((0.4 * baseDamage * (baseAtk / player.getDefense()))/50) + 2) * (Random.Range(85, 101) / 100);
        player.takeDamage((int)damage);
        return;
    }
    void Start()
    {
        player.enemiesInScene.Add(this);
        attackType = attackType.ToLower();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
