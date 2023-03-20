using UnityEngine;
using Photon.Pun;
using Team11.Players;
using Team11.Menus;

namespace Team11.Interactions
{
    public class LayerOfDeath : MonoBehaviour
    {
        public EscapeMenu menu;
        public float timeToRespawn = 3f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerBase>() == null) return;
            int objectID = other.GetComponent<PhotonView>().ViewID;
            if (!PhotonNetwork.GetPhotonView(objectID).IsMine) return;
            menu.Invoke(nameof(menu.CallResetLevel), timeToRespawn);
        }
    }
}