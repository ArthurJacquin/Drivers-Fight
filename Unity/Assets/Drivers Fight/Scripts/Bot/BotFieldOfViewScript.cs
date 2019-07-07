using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set states for bot reactions
/// Priority:
/// 1) Attack
/// 2) Collect
/// 3) Wander
/// </summary>
public class BotFieldOfViewScript : MonoBehaviour
{
    [SerializeField]
    private BotExposerScript bot;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = false;
            else if (bot.BotControllerScript.collectObject)
                bot.BotControllerScript.collectObject = false;

            bot.BotControllerScript.attackPlayer = true;
            bot.BotControllerScript.targetPosition = other.transform.position;
        }
        else if(other.tag == "Collectible" && !bot.BotControllerScript.attackPlayer)
        {
            if (bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = false;
            bot.BotControllerScript.collectObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bot.BotControllerScript.attackPlayer = false;
        if (other.tag == "Player")
        {
            if (!bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = true;
        }
        else if (other.tag == "Collectible" && !bot.BotControllerScript.attackPlayer)
        {
            bot.BotControllerScript.collectObject = false;
            if (!bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = true;
        }
    }
}
