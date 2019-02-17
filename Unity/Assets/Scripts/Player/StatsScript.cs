using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DriversFight.Scripts
{
    [System.Serializable]
    public class Stats
    {
        private List<float> modifiers = new List<float>();

        public float GetValue()
        {
            float finalValue = 0;
            modifiers.ForEach(x => finalValue += x);
            return finalValue;
        }

        public void AddModifier(float modifier)
        {
            if (modifier != 0)
            {
                modifiers.Add(modifier);
            }
        }

        public void RemoveModifier(float modifier)
        {
            if (modifier != 0)
            {
                modifiers.Remove(modifier);
            }
        }
    }
}