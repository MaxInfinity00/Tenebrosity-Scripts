using Photon.Pun;
using Team11.Players;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class Key : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject visual;

        private bool _acquired;
        
        public UnityEvent OnCollected;
        
        private void OnTriggerEnter(Collider other)
        {
            if(_acquired) return;
            if(other.GetComponent<PlayerBase>() == null) return;
            OnCollected?.Invoke();
            photonView.RPC(nameof(DisableVisual), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void DisableVisual()
        {
            _acquired = true;
            visual.SetActive(false);
        }
    }
}