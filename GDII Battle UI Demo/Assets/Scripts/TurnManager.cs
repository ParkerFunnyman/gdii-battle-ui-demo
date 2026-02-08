using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public bool IsPlayerTurn = true;
    public Player player;
    public List<Enemy> enemies = new List<Enemy>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (player.getCurrentHP() > 0)
        {
            enemies = player.enemiesInScene;
        }
    }
}
