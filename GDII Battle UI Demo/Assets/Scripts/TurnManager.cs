using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;
using Unity.VisualScripting;
using TMPro;
using System;
using UnityEngine.UI;
using System.Numerics;

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
    private bool battleOver = false;
    [SerializeField] private GameObject textbox;
    [SerializeField] private Transform canvas;
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
        battleOver = false;
        textbox.SetActive(false);
        flavortext.text = "if you're seeing this text I fucked up somewhere";
        currentState = TurnState.BattleStart;
        StartCoroutine(Battle());
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(currentState + " " + battleOver + " " + flavortext.text);
    }

    IEnumerator Battle()
    {
        while (!battleOver)
        {
            switch (currentState)
            {
                case TurnState.BattleStart:
                    battleOver = false;
                    string introText;
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
                    if ((enemies.Count <= 0) && (player.getCurrentHP() > 0))
                    {
                        currentState = TurnState.BattleWon;
                    }

                    //UI control
                    for (int i = 0; i < player.playerSpells.Count; i++)
                    {
                        Debug.Log(i);
                        Spell s = player.playerSpells[i];
                        GameObject newButton = Instantiate(button);
                        newButton.transform.SetParent(canvas, false);
                        //newButton.transform.position(new Vector3 (0,0,0));
                        //
                        //newButton.GetComponentInChildren<Text>().text = s.getSpellName();
                    }

                    //here for testing until attack ui is implemented
                    if (enemies.Count > 0)
                    {
                        player.playerSpells[0].castSpell(player, enemies[0]);
                        flavortext.text = "Rowan hit " + enemies[0].getName() + " with her staff!";
                    }

                    textbox.SetActive(true);
                    yield return new WaitForSeconds(1.5f); //for testing
                    textbox.SetActive(false);
                    yield return new WaitForSeconds(0.5f);

                    if (enemies.Count > 0)
                    {
                        for (int i = enemies.Count - 1; i >= 0; i--)
                        {
                            if (enemies[i].getCurrentHP() <= 0)
                            {
                                enemies[i].deathAnim();
                                enemies.RemoveAt(i);
                            }
                        }
                    }
                    if (enemies.Count > 0)
                    {
                        currentState = TurnState.EnemyTurn;
                    }
                    else
                    {
                        currentState = TurnState.BattleWon;
                    }
                    break;

                case TurnState.EnemyTurn:
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Enemy e = enemies[i];
                        EnemyAction a = e.actions[UnityEngine.Random.Range(0, e.actions.Count)];
                        if ((a.getType() == "light") && (e.getCurrentHP() >= e.getMaxHP()))
                        {
                            string t = e.getName() + " tried to heal, but its health was already full, so it attacked you instead!";
                            flavortext.text = t;
                        }
                        else
                        {
                            string t = e.getName() + " used " + a.getName() + "!";
                            flavortext.text = t;
                        }
                        a.doAction(enemies[i]);
                        textbox.SetActive(true);
                        yield return new WaitForSeconds(2f);
                        textbox.SetActive(false);
                        yield return new WaitForSeconds(0.5f);
                    }

                    if (player.getCurrentHP() <= 0)
                    {
                        currentState = TurnState.BattleLost;
                    }
                    else
                    {
                        currentState = TurnState.PlayerTurn;
                    }
                    break;

                case TurnState.BattleWon:
                    flavortext.text = "Oh yay yippee you win!!!";
                    textbox.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    textbox.SetActive(false);
                    battleOver = true;
                    //gain xp
                    //go back to dungeon scrawling
                    break;

                case TurnState.BattleLost:
                    player.die();
                    flavortext.text = "You lost! How tragic!";
                    textbox.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    textbox.SetActive(false);
                    battleOver = true;
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
