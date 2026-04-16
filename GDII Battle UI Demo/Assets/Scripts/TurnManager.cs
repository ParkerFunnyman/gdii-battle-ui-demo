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
using UnityEngine.SceneManagement;

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

    public enum PlayerTurnState
    {
        ListActions,
        ListSpells,
        ListItems,
        EnemySelect
    }

    public static TurnState currentState;
    public static PlayerTurnState currentPlayerState;
    private bool battleOver = false;
    private bool playerTurnOver = false;
    [SerializeField] private GameObject textbox;
    [SerializeField] private Transform canvas;
    [SerializeField] private TextMeshProUGUI flavortext;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject arrow;
    public Player player;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    private StatusUI status;
    private bool needInput = true;
    private Spell selectedSpell = null;
    private Enemy eSelected;
    private List<GameObject> buttons = new List<GameObject>();

    private float buttonScale = 4 / 3;
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
                    if (enemies.Count <= 0 && player.getCurrentHP() > 0)
                    {
                        currentState = TurnState.BattleWon;
                        break;
                    }

                    playerTurnOver = false;
                    needInput = true;
                    selectedSpell = null;
                    eSelected = enemies[0];
                    currentPlayerState = PlayerTurnState.ListActions;

                    yield return StartCoroutine(PlayerTurns());


                    if (selectedSpell != null)
                    {
                        selectedSpell.castSpell(player, eSelected);
                    }

                    if (selectedSpell == null)
                    {
                        //i should put something here
                    }
                    else if (selectedSpell.getSpellType() == "light")
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
                    yield return new WaitForSeconds(3.0f);
                    textbox.SetActive(false);
                    battleOver = true;
                    //gain xp
                    SceneManager.FieldTransition();
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
    IEnumerator PlayerTurns()
    {
        while (!playerTurnOver)
        {
            switch (currentPlayerState)
            {
                case PlayerTurnState.ListActions:
                    List<GameObject> actionButtons = new List<GameObject>();

                    //First button for staff attack
                    GameObject staffButton = Instantiate(button);
                    RectTransform SB = staffButton.GetComponent<RectTransform>();
                    staffButton.transform.SetParent(canvas, false);
                    SB.anchoredPosition = new UnityEngine.Vector2(704.50f, SB.anchoredPosition.y);
                    staffButton.GetComponentInChildren<TextMeshProUGUI>().text = "Staff Attack";
                    Button SBcomponent = staffButton.GetComponentInChildren<Button>();
                    SBcomponent.onClick.AddListener(delegate
                    {
                        selectedSpell = player.playerSpells[0];
                        currentPlayerState = PlayerTurnState.EnemySelect;
                    });
                    actionButtons.Add(staffButton);

                    //Second button for spell list
                    GameObject spellButton = Instantiate(button);
                    RectTransform SpellB = spellButton.GetComponent<RectTransform>();
                    spellButton.transform.SetParent(canvas, false);
                    SpellB.anchoredPosition = new UnityEngine.Vector2(704.50f, SpellB.anchoredPosition.y - gap);
                    spellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Spells";
                    Button SpellBcomponent = spellButton.GetComponentInChildren<Button>();
                    SpellBcomponent.onClick.AddListener(delegate
                    {
                        currentPlayerState = PlayerTurnState.ListSpells;
                    });
                    actionButtons.Add(spellButton);

                    //Third button for item list
                    GameObject itemButton = Instantiate(button);
                    RectTransform IB = itemButton.GetComponent<RectTransform>();
                    itemButton.transform.SetParent(canvas, false);
                    IB.anchoredPosition = new UnityEngine.Vector2(704.50f, IB.anchoredPosition.y - (2 * gap));
                    itemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Items";
                    Button IBcomponent = itemButton.GetComponentInChildren<Button>();
                    IBcomponent.onClick.AddListener(delegate
                    {
                        currentPlayerState = PlayerTurnState.ListItems;
                    });
                    actionButtons.Add(itemButton);

                    while (currentPlayerState == PlayerTurnState.ListActions)
                    {
                        yield return null;
                    }

                    for (int i = actionButtons.Count - 1; i >= 0; i--)
                    {
                        Destroy(actionButtons[i]);
                    }
                    break;

                case PlayerTurnState.ListSpells:
                    //Spawns a button for each spell in the player's spell array 
                    int mult = 0;
                    for (int i = 0; i < SceneManager.spells.Count; i++)
                    {
                        Spell s = SceneManager.spells[i];

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
                        yield return null;
                    }


                    //Removes buttons
                    for (int i = buttons.Count - 1; i >= 0; i--)
                    {
                        Destroy(buttons[i]);
                    }
                    currentPlayerState = PlayerTurnState.EnemySelect;
                    break;

                case PlayerTurnState.ListItems:
                    mult = 0;
                    for (int i = 0; i < SceneManager.items.Count; i++)
                    {
                        Item it = SceneManager.items[i];

                        GameObject newButton = Instantiate(button);
                        RectTransform rt = newButton.GetComponent<RectTransform>();

                        newButton.transform.SetParent(canvas, false);

                        float buttonY = rt.anchoredPosition.y - (mult * gap);
                        rt.anchoredPosition = new UnityEngine.Vector2(704.50f, buttonY);


                        newButton.GetComponentInChildren<TextMeshProUGUI>().text = it.getItemName();

                        Button buttonComponent = newButton.GetComponentInChildren<Button>();
                        buttonComponent.onClick.AddListener(delegate
                        {
                            it.useItem(player);
                            flavortext.text = "Rowan used a " + it.getItemName() + " on herself!";
                            SceneManager.items.Remove(it);

                            needInput = false;
                        });

                        buttons.Add(newButton);
                        mult++;

                    }

                    //Pauses game until attack is selected
                    while (needInput)
                    {
                        yield return null;
                    }

                    //Removes buttons
                    for (int i = buttons.Count - 1; i >= 0; i--)
                    {
                        Destroy(buttons[i]);
                    }
                    playerTurnOver = true;
                    break;

                case PlayerTurnState.EnemySelect:
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
                    playerTurnOver = true;
                    break;
                default:
                    currentPlayerState = PlayerTurnState.ListActions;
                    break;
            }
        }
        yield return null;
    }
}
