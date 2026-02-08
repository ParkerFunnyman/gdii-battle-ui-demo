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
