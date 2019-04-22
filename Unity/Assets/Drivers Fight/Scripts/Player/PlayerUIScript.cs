using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace DriversFight.Scripts
{
    public class PlayerUIScript : MonoBehaviour
    {
        [HideInInspector]
        public AvatarExposerScript avatar;

        [SerializeField]
        private Text speedText;

        [SerializeField]
        private Image healthBar;

        [SerializeField]
        private Image speedBar;

        // Update is called once per frame
        void Update()
        {
            speedText.text = Mathf.RoundToInt(avatar.Stats.currentSpeed).ToString();
            speedBar.fillAmount = avatar.Stats.currentSpeed / avatar.Stats.currentMaximumSpeed;
            healthBar.fillAmount = (float)avatar.Stats.EngineHealth / (float)avatar.Stats.EngineHealth;
        }
    }
}
