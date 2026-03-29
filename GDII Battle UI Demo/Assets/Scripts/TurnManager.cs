using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
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
    private bool battleOver = false;
    [SerializeField] private GameObject textbox;
    [SerializeField] private Transform canvas;
    [SerializeField] private TextMeshProUGUI flavortext;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject arrow;
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
    void Update()
    {
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
                    List<GameObject> buttons = new List<GameObject>();
                    bool needInput = true;
                    Enemy eSelected = enemies[0]; //enemy player selects

                    //redudant check for if an enemy has a self destruct spell or something
                    if ((enemies.Count <= 0) && (player.getCurrentHP() > 0))
                    {
                        currentState = TurnState.BattleWon;
                    }

                    //Spawns a button for each spell in the player's spell array
                    int mult = 0;
                    for (int i = 0; i < player.playerSpells.Count; i++)
                    {
                        Spell s = player.playerSpells[i];

                            GameObject newButton = Instantiate(button);
                            RectTransform rt = newButton.GetComponent<RectTransform>();

                            newButton.transform.SetParent(canvas, false);

                            float buttonY = rt.anchoredPosition.y - (mult * 90);
                            rt.anchoredPosition = new UnityEngine.Vector2(704.50f, buttonY);


                            newButton.GetComponentInChildren<TextMeshProUGUI>().text = s.getSpellName();

                            Button buttonComponent = newButton.GetComponentInChildren<Button>();
                            buttonComponent.onClick.AddListener(delegate
                            {
                                s.castSpell(player, eSelected);
                                if (s.getSpellType() == "light")
                                {
                                    flavortext.text = "Rowan used " + s.getSpellName() + "!";
                                }
                                else
                                {
                                    flavortext.text = "Rowan used " + s.getSpellName() + " on " + eSelected.getName() + "!";
                                }
                                needInput = false;
                            });

                            buttons.Add(newButton);
                            mult++;
                        
                    }

                    //Pauses game until attack is selected
                    while (needInput)
                    {
                        yield return new WaitForSeconds(0.02f);
                    }

                    //Removes buttons
                    for (int i = buttons.Count - 1; i >= 0; i--)
                    {
                        Destroy(buttons[i]);
                    }

                    //Displays textbox saying what spell was chosen
                    textbox.SetActive(true);
                    yield return new WaitForSeconds(1.5f); //for testing
                    textbox.SetActive(false);
                    yield return new WaitForSeconds(0.5f);

                    //makes enemy do their death animation if dead
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

                    //switches state
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
