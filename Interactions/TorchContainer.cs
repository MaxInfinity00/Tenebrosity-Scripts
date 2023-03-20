using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class TorchContainer : PlaceableObject, IContainer
    {
        [SerializeField] private Transform holder;
        public UnityEvent OnTorchPlaced;
        public UnityEvent OnTorchRemoved;

        private bool _firstPutIn;
        private Torch _currentPickup;

        public bool PutIn(IPickup pickup)
        {
            if (pickup is not Torch torch) return false;
            photonView.RPC(nameof(SetCurrentTorch), RpcTarget.AllBuffered, torch.photonView.ViewID);
            if (holder.childCount != 0 && !_firstPutIn)
            {
                _firstPutIn = true;
                return false;
            }
            
            _currentPickup.Place(holder);
            return true;
        }

        public IPickup TakeOut()
        {
            photonView.RPC(nameof(RemoveTorch), RpcTarget.AllBuffered);
            return _currentPickup;
        }

        [PunRPC]
        private void SetCurrentTorch(int photonViewID)
        {
            _currentPickup = PhotonNetwork.GetPhotonView(photonViewID).GetComponent<Torch>();
            _currentPickup.container = this;
            OnTorchPlaced.Invoke();
        }

        [PunRPC]
        private void RemoveTorch()
        {
            _currentPickup = null;
            OnTorchRemoved.Invoke();
        }
    }
}