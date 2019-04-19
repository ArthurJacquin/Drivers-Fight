using UnityEngine;

namespace DriversFight.Scripts
{
    public class AIntentReceiver : MonoBehaviour
    {
        public bool WantToMoveBackward { get; set; }
        public bool WantToMoveForward { get; set; }
        public bool WantToMoveLeft { get; set; }
        public bool WantToMoveRight { get; set; }
        public bool WantToStopTheCar { get; set; }
    }
}
