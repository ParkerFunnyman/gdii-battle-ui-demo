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
    private StatusUI status;

    private float buttonScale = 4/3;
    private int gap = 125;

    public void addEnemy(Enemy e)
    {
        enemies.Add(e);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].setPlayer(player);
        }
        status = canvas.GetComponentInChildren<StatusUI>();
        status.SetMaxHealth(player.getMaxHP(), player.getMaxMana());
        battleOver = false;
        textbox.SetActive(false);
        flavortext.text = "if you're seeing this text I fucked up somewhere";
        currentState = TurnState.BattleStart;
        StartCoroutine(Battle());
    }

    // Update is called once per frame
    void Update()
    {
        status.SetHealth(player.getCurrentHP());
        status.SetMana(player.getCurrentMana());
    }


    IEnumerator Battle()
    {
        while (!battleOver)
        {
            switch (currentState)
            {
                case TurnState.BattleStart:
                    battleOver = false;

                    //Beginning of battle text
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

                    //Switch to PlayerTurn
                    currentState = TurnState.PlayerTurn;
                    break;

                case TurnState.PlayerTurn:
                    List<GameObject> buttons = new List<GameObject>();
                    bool needInput = true;
                    Spell selectedSpell = null;
                    Enemy eSelected = enemies[0]; //enemy player selects
                    int PlayerTurnState = 0;

                    //redudant check for if an enemy has a self destruct spell or something
                    if ((enemies.Count <= 0) && (player.getCurrentHP() > 0))
                    {
                        currentState = TurnState.BattleWon;
                    }

                    // //Handle internal states of the PlayerTurn state
                    // while (PlayerTurnState != 4)
                    // {
                    //     switch (PlayerTurnState)
                    //     {
                    //         case 0: //list potential actions
                    //             int mult1 = 0;
                    //             for (int i = 1; i < player.playerSpells.Count; i++)
                    //             {
                    //                 Spell s = player.playerSpells[i];

                    //                 GameObject newButton = Instantiate(button);
                    //                 RectTransform rt = newButton.GetComponent<RectTransform>();

                    //                 newButton.transform.SetParent(canvas, false);

                    //                 float buttonY = rt.anchoredPosition.y - (mult1 * 90);
                    //                 rt.anchoredPosition = new UnityEngine.Vector2(704.50f, buttonY);


                    //                 newButton.GetComponentInChildren<TextMeshProUGUI>().text = s.getSpellName();

                    //                 Button buttonComponent = newButton.GetComponentInChildren<Button>();
                    //                 switch (i)
                    //                 {
                    //                     case 1:
                    //                         buttonComponent.onClick.AddListener(delegate
                    //                         {
                    //                             selectedSpell = player.playerSpells[0];
                    //                             PlayerTurnState = 2;
                    //                         });
                    //                         break;
                    //                     case 2:
                    //                         buttonComponent.onClick.AddListener(delegate
                    //                         {
                    //                             PlayerTurnState = 1;
                    //                         });
                    //                         break;
                    //                     case 3:
                    //                         buttonComponent.onClick.AddListener(delegate
                    //                         {
                    //                             Debug.Log("Items!");
                    //                         });
                    //                         break;
                    //                     default:
                    //                         break;
                    //                 }

                    //                 buttons.Add(newButton);
                    //                 mult1++;

                    //             }

                    //         // //Removes buttons
                    //         // for (int i = buttons.Count - 1; i >= 0; i--)
                    //         // {
                    //         //     Destroy(buttons[i]);
                    //         // }
                    //         // break;

                    //         case 1: //list spells
                    //             int mult2 = 0;
                    //             for (int i = 1; i < player.playerSpells.Count; i++)
                    //             {
                    //                 Spell s = player.playerSpells[i];

                    //                 GameObject newButton = Instantiate(button);
                    //                 RectTransform rt = newButton.GetComponent<RectTransform>();

                    //                 newButton.transform.SetParent(canvas, false);

                    //                 float buttonY = rt.anchoredPosition.y - (mult2 * 90);
                    //                 rt.anchoredPosition = new UnityEngine.Vector2(704.50f, buttonY);


                    //                 newButton.GetComponentInChildren<TextMeshProUGUI>().text = s.getSpellName();

                    //                 Button buttonComponent = newButton.GetComponentInChildren<Button>();
                    //                 buttonComponent.onClick.AddListener(delegate
                    //                 {

                    //                     if (s.getManaCost() <= player.getCurrentMana())
                    //                     {
                    //                         selectedSpell = s;
                    //                         needInput = false;
                    //                     }
                    //                     else
                    //                     {
                    //                         flavortext.text = "Rowan does not have enough MANA to cast this spell.";
                    //                         textbox.SetActive(true);
                    //                     }
                    //                 });

                    //                 buttons.Add(newButton);
                    //                 mult2++;

                    //             }

                    //             if (Keyboard.current.backspaceKey.isPressed)
                    //             {
                    //                 PlayerTurnState = 0;
                    //                 needInput = false;
                    //             }
                    //             //Pauses game until attack is selected
                    //             while (needInput)
                    //             {
                    //                 yield return new WaitForSeconds(0.02f);
                    //             }

                    //             //Removes buttons
                    //             for (int i = buttons.Count - 1; i >= 0; i--)
                    //             {
                    //                 Destroy(buttons[i]);
                    //             }
                    //             break;

                    //         case 2: //enemy select
                    //             if (enemies.Count > 1 && selectedSpell.getSpellType() != "light")
                    //             {
                    //                 //while enter not pressed
                    //                 int selectIndex = 0;
                    //                 GameObject arrowToEnemy = Instantiate(arrow);
                    //                 UnityEngine.Vector3 offset = new UnityEngine.Vector3(0, 2, 0);

                    //                 bool selecting = true;
                    //                 arrowToEnemy.transform.position = enemies[0].getPosition() + offset;
                    //                 while (selecting)
                    //                 {

                    //                     if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
                    //                     {
                    //                         selectIndex--;
                    //                         if (selectIndex < 0)
                    //                         {
                    //                             selectIndex = enemies.Count - 1;
                    //                         }
                    //                         else if (selectIndex >= enemies.Count)
                    //                         {
                    //                             selectIndex = 0;
                    //                         }
                    //                         arrowToEnemy.transform.position = enemies[selectIndex].getPosition() + offset;
                    //                     }
                    //                     else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
                    //                     {
                    //                         selectIndex++;
                    //                         if (selectIndex < 0)
                    //                         {
                    //                             selectIndex = enemies.Count - 1;
                    //                         }
                    //                         else if (selectIndex >= enemies.Count)
                    //                         {
                    //                             selectIndex = 0;
                    //                         }
                    //                         arrowToEnemy.transform.position = enemies[selectIndex].getPosition() + offset;
                    //                     }
                    //                     else if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.zKey.wasPressedThisFrame)
                    //                     {
                    //                         eSelected = enemies[selectIndex];
                    //                         selecting = false;
                    //                     }
                    //                     //DO LATER
                    //                     //return to previous menu
                    //                     else if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                    //                     {
                    //                         PlayerTurnState = 0;
                    //                         selecting = false;
                    //                     }
                    //                     yield return null;
                    //                 }

                    //                 Destroy(arrowToEnemy);
                    //             }
                    //             //If only one enemy, don't bother running script
                    //             else
                    //             {
                    //                 eSelected = enemies[0];
                    //             }
                    //             break;
                    //         case 3: //
                    //             break;
                    //         case 4:
                    //             break;
                    //         default:
                    //             break;
                    //     }
                    // }

                    // //wait until spell is chosen

                    //castSpell(eSelected, player)

                    //Spawns a button for each spell in the player's spell array 
                    int mult = 0;
                    for (int i = 0; i < player.playerSpells.Count; i++)
                    {
                        Spell s = player.playerSpells[i];

                        GameObject newButton = Instantiate(button);
                        RectTransform rt = newButton.GetComponent<RectTransform>();

                        newButton.transform.SetParent(canvas, false);

                        float buttonY = rt.anchoredPosition.y - (mult * gap);
                        rt.anchoredPosition = new UnityEngine.Vector2(704.50f, buttonY);


                        newButton.GetComponentInChildren<TextMeshProUGUI>().text = s.getSpellName();

                        Button buttonComponent = newButton.GetComponentInChildren<Button>();
                        buttonComponent.onClick.AddListener(delegate
                        {

                            if (s.getManaCost() <= player.getCurrentMana())
                            {
                                selectedSpell = s;
                                needInput = false;
                            }
                            else
                            {
                                flavortext.text = "Rowan does not have enough MANA to cast this spell.";
                                textbox.SetActive(true);
                            }
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

                    //Enemy select
                    if (enemies.Count > 1 && selectedSpell.getSpellType() != "light")
                    {
                        //while enter not pressed
                        int selectIndex = 0;
                        GameObject arrowToEnemy = Instantiate(arrow);
                        UnityEngine.Vector3 offset = new UnityEngine.Vector3(0, 2, 0);

                        bool selecting = true;
                        arrowToEnemy.transform.position = enemies[0].getPosition() + offset;
                        while (selecting)
                        {

                            if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
                            {
                                selectIndex--;
                                if (selectIndex < 0)
                                {
                                    selectIndex = enemies.Count - 1;
                                }
                                else if (selectIndex >= enemies.Count)
                                {
                                    selectIndex = 0;
                                }
                                arrowToEnemy.transform.position = enemies[selectIndex].getPosition() + offset;
                            }
                            else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
                            {
                                selectIndex++;
                                if (selectIndex < 0)
                                {
                                    selectIndex = enemies.Count - 1;
                                }
                                else if (selectIndex >= enemies.Count)
                                {
                                    selectIndex = 0;
                                }
                                arrowToEnemy.transform.position = enemies[selectIndex].getPosition() + offset;
                            }
                            else if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.zKey.wasPressedThisFrame)
                            {
                                eSelected = enemies[selectIndex];
                                selecting = false;
                            }
                            //DO LATER
                            //return to previous menu
                            else if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                            {
                                eSelected = enemies[selectIndex];
                                selecting = false;
                            }
                            yield return null;
                        }

                        Destroy(arrowToEnemy);
                    }
                    //If only one enemy, don't bother running script
                    else
                    {
                        eSelected = enemies[0];
                    }

                    //Displays textbox saying what spell was chosen
                    selectedSpell.castSpell(player, eSelected);
                    if (selectedSpell.getSpellType() == "light")
                    {
                        flavortext.text = "Rowan used " + selectedSpell.getSpellName() + "!";
                    }
                    else
                    {
                        flavortext.text = "Rowan used " + selectedSpell.getSpellName() + " on " + eSelected.getName() + "!";
                    }
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
                        EnemyAction a = null;

                        //Enemy AI behavior:
                        //Pick a random spell from their list of spells
                        //If that attack is a healing spell while they have more than 25% HP, try again
                        if (e.getCurrentHP() < (e.getMaxHP() * 0.25))
                        {
                            a = e.actions[UnityEngine.Random.Range(0, e.actions.Count)];
                        }
                        else
                        {
                            a = e.actions[UnityEngine.Random.Range(0, e.actions.Count)];
                            while (a.getType() == "light")
                            {
                                a = e.actions[UnityEngine.Random.Range(0, e.actions.Count)];
                            }
                        }

                        //Enemy textbox
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

                    //Switches state
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
                    //replace random xp with actual function
                    flavortext.text = "All enemies are defeated! You gained " + UnityEngine.Random.Range(40, 50) + " XP.";
                    textbox.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    textbox.SetActive(false);
                    battleOver = true;
                    //gain xp
                    //go back to dungeon scrawling
                    break;

                case TurnState.BattleLost:
                    player.die();
                    flavortext.text = "You've perished.";
                    textbox.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    textbox.SetActive(false);
                    battleOver = true;
                    Application.Quit();//replace with scene change or something of the like
                    break;

                default:
                    //hopefully this never gets used.
                    //hopefully.
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
