using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;
using Unity.VisualScripting;
using TMPro;
using System;
using UnityEngine.UI;

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

    [SerializeField] private GameObject textbox;
    [SerializeField] private TextMeshProUGUI flavortext;
    public GameObject button;
    public Player player;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    
    public void addEnemy(Enemy e)
    {
        enemies.Add(e);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textbox.SetActive(false);
        flavortext.text = "if you're seeing this text I fucked up somewhere";
        currentState = TurnState.BattleStart;
        StartCoroutine(Battle());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Battle()
    {
        while (currentState != TurnState.BattleWon && currentState != TurnState.BattleLost)
        {
            switch (currentState)
            {
                case TurnState.BattleStart:
                    String introText;
                    if (enemies.Count > 1)
                    {
                        introText = "Multiple enemies came out of nowhere!";
                    }
                    else if (enemies.Count == 1)
                    {
                        introText = "A wild " + enemies[0].getName() + " has appeared!";
                    }
                    else
                    {
                        introText = "Hey wait a minute there should be enemies here";
                    }
                    flavortext.text = introText;
                    textbox.SetActive(true);
                    yield return new WaitForSeconds(3f);
                    textbox.SetActive(false);
                    currentState = TurnState.PlayerTurn;
                    break;

                case TurnState.PlayerTurn:
                    if (player.getCurrentHP() == 0)
                    {
                        currentState = TurnState.BattleLost;
                    }
                    //UI control
                    for (int i = 0; i < player.playerSpells.Count; i++)
                    {
                        Vector3 spawnlocation = new Vector3(705, (440 - i*75), 0);
                    }
                    if ((enemies.Count <= 0) && (player.getCurrentHP() > 0))
                    {
                        currentState = TurnState.BattleWon;
                    }
                    yield return new WaitForSeconds(1.5f); //for testing
                    currentState = TurnState.EnemyTurn;
                    break;

                case TurnState.EnemyTurn:
                    for (int i = 0; i < enemies.Count; i++)
                        {
                            EnemyAction a = enemies[i].actions[UnityEngine.Random.Range(0, (enemies[i].actions.Count))];
                            a.doAction(enemies[i]);
                            String t = enemies[i].getName() + " used " + a.getName() +"!";
                            flavortext.text = t;
                            textbox.SetActive(true);
                            yield return new WaitForSeconds(1.5f);
                            textbox.SetActive(false);
                        }
                        if (player.getCurrentHP() <= 0)
                        {
                            currentState = TurnState.BattleLost;
                        }
                        if ((enemies.Count <= 0) && (player.getCurrentHP() > 0))
                        {
                            currentState = TurnState.BattleWon;
                        }
                        currentState = TurnState.PlayerTurn;
                    break;
                    
                case TurnState.BattleWon:
                    //gain xp
                    //go back to dungeon scrawling
                    break;

                case TurnState.BattleLost:
                    flavortext.text = "You lost! How tragic!";
                    player.die();
                    yield return new WaitForSeconds(1f);
                    textbox.SetActive(true);
                    Application.Quit();//replace with scene change or something of the like
                    break;

                default:
                    textbox.SetActive(true);
                    flavortext.text = "Erm, you aren't supposed to see this! How embarrasing!";
                    Debug.Log("How did we get here.");
                    currentState = TurnState.BattleStart;
                    break;
            }
        }
        yield return null; 
    }
}
