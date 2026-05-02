using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.Rendering;
using System.Linq;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class FieldPlayer : MonoBehaviour
{
    public enum menu_state
    {
        main,
        spell_list,
    }
    private List<GameObject> buttons = new List<GameObject>();

    private bool needInput = true;
    private Spell selectedSpell = null;
    public Player player;
    public static menu_state current_menu_state;
    private float buttonScale = 4 / 3;
    private int gap = 125;
    public static CharacterController controller;
    public static Vector3 horizontalVelocity;
    [SerializeField] private GameObject button;
    [SerializeField] private Transform canvas;
    private static int steps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_menu_state = menu_state.main;
        controller = GetComponent<CharacterController>();
        steps = 0;
        if (SceneManager.restorePos)
        {
            transform.position = SceneManager.playerPos;
        }

        current_menu_state = menu_state.main;
        StartCoroutine(spellcast());
    }

    // Update is called once per frame
    void Update()
    {
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        bool isWalking = horizontalVelocity.magnitude > 0.1f;
        if (isWalking)
        {
            if (steps != 2000)
            {
                Debug.Log(steps);
                steps++;
                return;
            }

            int rand = Random.Range(0, 1000);
            if (rand > 998)
            {
                steps = 0;
                SceneManager.BattleTransition(transform.position);
            }
        }
    }

    IEnumerator spellcast()
    {
        switch(current_menu_state)
        {
            case menu_state.main:
                List<GameObject> actionButtons = new List<GameObject>();

                //Second button for spell list
                GameObject spellButton = Instantiate(button);
                RectTransform SpellB = spellButton.GetComponent<RectTransform>();
                spellButton.transform.SetParent(canvas, false);
                SpellB.anchoredPosition = new UnityEngine.Vector2(704.50f, SpellB.anchoredPosition.y);
                spellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Spells";
                Button SpellBcomponent = spellButton.GetComponentInChildren<Button>();
                SpellBcomponent.onClick.AddListener(delegate
                {
                    //currentPlayerState = PlayerTurnState.ListSpells;
                });
                actionButtons.Add(spellButton);

                //Third button for item list
                GameObject itemButton = Instantiate(button);
                RectTransform IB = itemButton.GetComponent<RectTransform>();
                itemButton.transform.SetParent(canvas, false);
                IB.anchoredPosition = new UnityEngine.Vector2(704.50f, IB.anchoredPosition.y - gap);
                itemButton.GetComponentInChildren<TextMeshProUGUI>().text = "Items";
                Button IBcomponent = itemButton.GetComponentInChildren<Button>();
                IBcomponent.onClick.AddListener(delegate
                {
                    //currentPlayerState = PlayerTurnState.ListItems;
                });
                actionButtons.Add(itemButton);

                while (current_menu_state == menu_state.main)
                {
                    yield return null;
                }

                for (int i = actionButtons.Count - 1; i >= 0; i--)
                {
                    Destroy(actionButtons[i]);
                }
                break;
                

            case menu_state.spell_list:
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
                            //flavortext.text = "Rowan does not have enough MANA to cast this spell.";
                            //textbox.SetActive(true);
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

                current_menu_state = menu_state.main;
                break;

            
            default:
                current_menu_state = menu_state.main;
                break;
        }
    }
}
