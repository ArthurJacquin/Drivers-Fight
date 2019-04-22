using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DriversFight.Scripts
{
    public class DisplayItemButton : MonoBehaviour
    {
        public CarStatsScript carItem;
        public int itemNumber;

        public Text itemName;
        public Text itemCost;
        public Text itemDescription;

        // Start is called before the first frame update
        void Start()
        {
            SetButton();
        }

        void SetButton()
        {
            itemName.text = "yes";
            itemCost.text = "maybe";
            itemDescription.text = "no";
            /*itemName.text = carItem.items[itemNumber].name;
            itemCost.text = "$" + carItem.items[itemNumber].cost;
            itemDescription.text = carItem.items[itemNumber].description;*/
        }

        public void OnClick()
        {
            /*if(carItem.currentEngineHealth > carItem.items[itemNumber].cost)
            {
                carItem.currentEngineHealth -= carItem.items[itemNumber].cost;
                carItem.currentItem = itemNumber;
            }*/
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
    
