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
        if(other.tag == "Car")
        {
            if (bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = false;
            if (bot.BotControllerScript.dodgeWall)
                bot.BotControllerScript.dodgeWall = false;

            bot.BotControllerScript.attackPlayer = true;
            bot.BotControllerScript.targetObject = other.gameObject;
        }

        if(other.tag == "Wall" && bot.BotControllerScript.wander)
        {
            bot.BotControllerScript.dodgeWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bot.BotControllerScript.attackPlayer = false;
        if (other.tag == "Car")
        {
            if (!bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = true;
            bot.BotControllerScript.targetObject = null;
        }

        if (other.tag == "Wall" && bot.BotControllerScript.wander)
        {
            bot.BotControllerScript.dodgeWall = false;

            if(!bot.BotControllerScript.wander)
                bot.BotControllerScript.wander = true;
        }
    }
}
