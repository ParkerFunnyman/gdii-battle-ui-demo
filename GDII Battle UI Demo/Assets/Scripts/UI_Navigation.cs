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
using System.Runtime.CompilerServices;

public class UI_Navigation : MonoBehaviour
{
    public enum menu_state
    {
        main,
        spell_list,
        item_list,
    }
    private List<GameObject> buttons = new List<GameObject>();
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject arrowUI;

    public Player player;

    private bool needInput = true;
    private Spell selectedSpell = null;
    public static menu_state current_menu_state;
    public static menu_state previous_menu_state;
    private float buttonScale = 4 / 3;
    private int gap = 125;
    [SerializeField] private GameObject button;
    [SerializeField] private Transform canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current_menu_state = menu_state.main;
        StartCoroutine(spellcast());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        IEnumerator spellcast()
    {
        while (true)
        {
            switch(current_menu_state)
            {
                case menu_state.main:
                    int buttonSelect = 0;
                        bool keyboardSelecting = true;
                        GameObject arrowSelector = Instantiate(arrowUI);
                        arrowSelector.transform.SetParent(canvas, false);

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
                            current_menu_state = menu_state.spell_list;
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
                            current_menu_state = menu_state.item_list;
                        });
                        actionButtons.Add(itemButton);

                        while ((current_menu_state == menu_state.main) && (keyboardSelecting))
                        {
                            if (buttonSelect < 0)
                            {
                                buttonSelect = actionButtons.Count - 1;
                            }
                            else if (buttonSelect >= actionButtons.Count){
                                buttonSelect = 0;
                            }

                            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                            {
                                buttonSelect--;
                            }else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
                            {
                                buttonSelect++;
                            }else if (Keyboard.current.enterKey.wasPressedThisFrame)
                            {
                                keyboardSelecting = false;
                            }
                            arrowSelector.transform.position = new UnityEngine.Vector2(1375.0f, 1100 - (gap * (buttonSelect+1)));
                            yield return null;
                        }

                        if (!keyboardSelecting)
                        {
                            switch (buttonSelect)
                            {
                                case 0:
                                    current_menu_state = menu_state.spell_list;
                                    break;
                                case 1:
                                    current_menu_state = menu_state.item_list;
                                    break;
                                default:
                                    current_menu_state = menu_state.main;
                                    break;
                            }
                        }
                        for (int i = actionButtons.Count - 1; i >= 0; i--)
                        {
                            Destroy(actionButtons[i]);
                        }
                        Destroy(arrowSelector);
                        break;

                case menu_state.spell_list:
                    buttonSelect = 0;
                    keyboardSelecting = true;
                    arrowSelector = Instantiate(arrowUI);
                    arrowSelector.transform.SetParent(canvas, false);

                    //Spawns a button for each spell in the player's spell array 
                    int mult = 0;
                    needInput = true;

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
                            selectedSpell = s;
                            needInput = false;
                        });

                        buttons.Add(newButton);
                        mult++;

                    }

                    //Pauses game until attack is selected
                    while ((needInput) && (keyboardSelecting))
                    {
                        if (buttonSelect < 0)
                        {
                            buttonSelect = buttons.Count - 1;
                        }
                        else if (buttonSelect >= buttons.Count)
                        {
                            buttonSelect = 0;
                        }

                        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                        {
                            buttonSelect--;
                        }
                        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
                        {
                            buttonSelect++;
                        }
                        else if (Keyboard.current.enterKey.wasPressedThisFrame)
                        {
                            keyboardSelecting = false;
                        }
                        arrowSelector.transform.position = new UnityEngine.Vector2(1375.0f, 1100 - (gap * (buttonSelect + 1)));

                        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                        {
                            needInput = false;
                        }
                        
                        yield return null;
                    }

                    if (!keyboardSelecting)
                    {
                        selectedSpell = SceneManager.spells[buttonSelect];
                    }

                    //Removes buttons
                    for (int i = buttons.Count - 1; i >= 0; i--)
                    {
                        Destroy(buttons[i]);
                    }
                    Destroy(arrowSelector);
                    if (selectedSpell != null)
                    {
                        previous_menu_state = menu_state.spell_list;
                        current_menu_state = menu_state.main;
                    }
                    break;

                case menu_state.item_list:
                    buttonSelect = 0;
                    keyboardSelecting = true;
                    arrowSelector = Instantiate(arrowUI);
                    arrowSelector.transform.SetParent(canvas, false);
                    mult = 0;
                    needInput = true;
                    bool usedItem = false;
                    List<Item> possibleItems = new List<Item>();
                    for (int i = 0; i < SceneManager.items.Distinct().Count(); i++)
                    {
                        Item it = SceneManager.items.Distinct().ElementAt(i);
                        possibleItems.Add(it);
                        GameObject newButton = Instantiate(button);
                        RectTransform rt = newButton.GetComponent<RectTransform>();

                        newButton.transform.SetParent(canvas, false);

                        float buttonY = rt.anchoredPosition.y - (mult * gap);
                        rt.anchoredPosition = new UnityEngine.Vector2(704.50f, buttonY);


                        newButton.GetComponentInChildren<TextMeshProUGUI>().text = it.getItemName() + ": x" + SceneManager.items.Count(x => x.getItemName().Contains(it.getItemName()));

                        Button buttonComponent = newButton.GetComponentInChildren<Button>();
                        buttonComponent.onClick.AddListener(delegate
                        {
                            it.useItem(player);
                            SceneManager.items.Remove(it);
                            usedItem = true;  // set it here
                            needInput = false;
                        });

                        buttons.Add(newButton);
                        mult++;

                    }

                    //Pauses game until attack is selected
                    while ((needInput) && (keyboardSelecting))
                    {
                        if (buttonSelect < 0)
                        {
                            buttonSelect = buttons.Count - 1;
                        }
                        else if (buttonSelect >= buttons.Count)
                        {
                            buttonSelect = 0;
                        }

                        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                        {
                            buttonSelect--;
                        }
                        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
                        {
                            buttonSelect++;
                        }
                        else if (Keyboard.current.enterKey.wasPressedThisFrame)
                        {
                            keyboardSelecting = false;
                        }
                        arrowSelector.transform.position = new UnityEngine.Vector2(1375.0f, 1100 - (gap * (buttonSelect + 1)));

                        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                        {
                            needInput = false;
                        }
                        yield return null;
                    }

                    if (!keyboardSelecting)
                    {
                        Item used = possibleItems[buttonSelect];
                        used.useItem(player);
                        SceneManager.items.Remove(used);
                    }

                    for (int i = buttons.Count - 1; i >= 0; i--)
                    {
                        Destroy(buttons[i]);
                    }

                    Destroy(arrowSelector);
                    if (usedItem)
                    {
                        previous_menu_state = menu_state.spell_list;
                        current_menu_state = menu_state.main;
                    }
                    break;

                default:
                    current_menu_state = menu_state.main;
                    break;
            }

            yield return null;
        }
        
    }
}
