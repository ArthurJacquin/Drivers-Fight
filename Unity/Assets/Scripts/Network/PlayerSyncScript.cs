using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace DriversFight.Scripts
{
    public class PlayerSyncScript : MonoBehaviour, IPunObservable
    {
        private Vector3 correctPlayerPos;
        private Quaternion correctPlayerRot;

        [SerializeField]
        private PhotonView view;

        void Update()
        {
            if (!view.IsMine)
            {
                this.transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                this.correctPlayerPos = (Vector3)stream.ReceiveNext();
                this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
