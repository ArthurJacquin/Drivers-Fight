using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Invincibility Buff")]
public class InvincibilityBuffItemEffect : UsableItemEffect
{
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.Invincibility = true;
        character.StartCoroutine(RemoveBuff(character, Duration));
    }

    public override string GetDescription()
    {
        if (Application.systemLanguage == SystemLanguage.French)
        {
            return "Vous êtes invincible pour " + Duration + " secondes.";
        }

        return "You are invincible for " + Duration + " seconds.";
    }

    private static IEnumerator RemoveBuff(Character character, float duration)
    {
        yield return new WaitForSeconds(duration);
        character.Invincibility = false;
    }
}