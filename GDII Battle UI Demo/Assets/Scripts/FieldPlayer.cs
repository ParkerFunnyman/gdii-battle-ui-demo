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
    public Player player;
    public static CharacterController controller;
    public static Vector3 horizontalVelocity;
    private static int steps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        steps = 0;
        if (SceneManager.restorePos)
        {
            transform.position = SceneManager.playerPos;
            player.setCurrentMana(SceneManager.playerMana);
            player.setCurrentHP(SceneManager.playerHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        bool isWalking = horizontalVelocity.magnitude > 0.1f;
        if (isWalking)
        {
            if (steps != 1500)
            {
                Debug.Log(steps);
                steps++;
                return;
            }

            int rand = Random.Range(0, 1000);
            if (rand > 990)
            {
                steps = 0;
                SceneManager.BattleTransition(transform.position);
            }
        }
    }
}
