using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class Lever : MonoBehaviourPunCallbacks, IInteractable
    {
        public UnityEvent OnPress;
        public UnityEvent OnRevert;

        private bool _pushed;

        public void Interact()
        {
            if (_pushed) return;
            photonView.RPC(nameof(Toggle), RpcTarget.AllBuffered);
        }

        public void Revert()
        {
            if (!_pushed) return;
            Toggle();
        }

        [PunRPC]
        public void Toggle()
        {
            _pushed = !_pushed;
            if (_pushed)
            {
                OnPress?.Invoke();
            }
            else
            {
                OnRevert?.Invoke();
            }
        }
    }
}