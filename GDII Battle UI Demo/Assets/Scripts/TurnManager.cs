using UnityEngine;
using System.Collections;
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
        BattleWon,
        BattleLost
    }

    public static TurnState currentState;

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

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = TurnState.BattleStart;
        StartCoroutine(Battle());
    }

    // Update is called once per frame
    void Update()
    {
        enemies = player.enemiesInScene;
    }

    IEnumerator Battle()
    {
        while (currentState != TurnState.BattleWon && currentState != TurnState.BattleLost)
        {
            switch (currentState)
            {
                case TurnState.BattleStart:
                    //Spawn text box
                    yield return new WaitForSeconds(1f);
                    currentState = TurnState.PlayerTurn;
                    break;

                case TurnState.PlayerTurn:
                    currentState = TurnState.BattleWon;
                    //UI control
                    if ((enemies.Count <= 0) && (player.getCurrentHP() > 0))
                    {
                        currentState = TurnState.BattleWon;
                    }
                    break;

                case TurnState.EnemyTurn:
                    for (int i = 0; i < enemies.Count; i++)
                        {
                            enemies[i].actions[Random.Range(0, (enemies[i].actions.Count - 1))].doAction(enemies[i]);
                        }
                        IsPlayerTurn = true;
                    break;
                    
                case TurnState.BattleWon:
                    break;

                case TurnState.BattleLost:
                    player.die();
                    break;

                default:
                    Debug.Log("How did we get here.");
                    currentState = TurnState.BattleStart;
                    break;
            }
        }
        yield return null; 
    }
}
