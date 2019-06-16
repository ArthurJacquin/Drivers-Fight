using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DriversFight.Scripts
{
    public class DisplayItemButton : MonoBehaviour
    {
        // Par Benoît : CarStatsScript est un script obsolete périmé depuis longtemps, pourquoi l'insérer ici ? Celui-ci n'existe plus.
        // public CarStatsScript carItem;

        public int itemNumber;

        public Text itemName;
        public Text itemCost;
        public Text itemDescription;

        [HideInInspector]
        public AvatarExposerScript avatar;

        // Start is called before the first frame update
        void Start()
        {
            SetButton();
        }

        void SetButton()
        {
            itemName.text = "Repair car";
            itemCost.text = "100";
        }

        public void OnClick()
        {
            if(avatar.Stats.EngineHealth < 250)
            {
                avatar.Stats.EngineHealth = avatar.Stats.EngineHealth + 100;
                Debug.Log("+100");
            }
            
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
    
