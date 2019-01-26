using UnityEngine;

namespace MyPhotonProject.Scripts
{
    public class AIntentReceiver : MonoBehaviour
    {
        public bool WantToMoveBack { get; set; }
        public bool WantToMoveForward { get; set; }
        public bool WantToMoveLeft { get; set; }
        public bool WantToMoveRight { get; set; }
    }
}
