using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;

public class TurnManager : MonoBehaviour
{
    public bool IsPlayerTurn = true;
    public Player player;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = player.enemiesInScene;
    }
}
