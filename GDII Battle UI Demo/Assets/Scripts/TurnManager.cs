using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;

public class TurnManager : MonoBehaviour
{
    public enum TurnState
    {
        BattleStart,
        PlayerTurn,
        EnemyTurn,
    }
    public bool IsPlayerTurn = true;
    public Player player;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    private void EnemyTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].actions[Random.Range(0, (enemies[i].actions.Count - 1))].doAction(enemies[i]);
        }
        IsPlayerTurn = true;
    }

    private void PlayerTurn()
    {
        //DO LATER
        IsPlayerTurn = false;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = player.enemiesInScene;
    }
}
