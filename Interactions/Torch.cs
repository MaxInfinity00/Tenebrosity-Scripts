using Photon.Pun;
using UnityEngine;
using FMODUnity;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
    public class Torch : PickupBase
    {
        public TorchContainer container;
        [SerializeField] private float smoothTime = 0.5f;
        [Range(1, 15)] [SerializeField] private float range = 3;

        private Vector3 _startPos;
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _startPos = transform.position;
            if (container != null)
                container.PutIn(this);
        }

        public override void Pickup(Transform tr)
        {
            if (container != null)
                container.TakeOut();
            photonView.RPC(nameof(SetLayer), RpcTarget.AllBuffered, "Ignore Raycast");
            photonView.RPC(nameof(SetColliderTrigger), RpcTarget.AllBuffered, true);
            photonView.RPC(nameof(SetTorchParent), RpcTarget.AllBuffered, tr.gameObject.GetPhotonView().ViewID);
            MoveTo(false);
            RuntimeManager.PlayOneShot("event:/Interactables/Light Grab");
        }

        public override void Place(Transform tr)
        {
            if (tr == null)
            {
                if (container != null)
                    container.PutIn(this);
                else
                    MoveTo(true, _startPos);
                return;
            }

            photonView.RPC(nameof(SetTorchParent), RpcTarget.AllBuffered, tr.gameObject.GetPhotonView().ViewID);
            MoveTo(true);
            RuntimeManager.PlayOneShot("event:/Interactables/Light Let Go");
        }

        [PunRPC]
        public void SetTorchParent(int photonViewID)
        {
            transform.SetParent(PhotonNetwork.GetPhotonView(photonViewID).transform);
        }

        [PunRPC]
        public void SetLayer(string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
        }

        [PunRPC]
        public void SetColliderTrigger(bool active)
        {
            _collider.isTrigger = active;
        }

        public void UpdateRange()
        {
            transform.GetChild(0).localScale = Vector3.one * range;
            GetComponent<Light>().range = range / 2f;
        }

        private void MoveTo(bool deActiveCollider, Vector3 dest = default)
        {
            LeanTween.moveLocal(gameObject, dest, smoothTime).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>
            {
                transform.rotation = Quaternion.identity;
                if (deActiveCollider)
                {
                    photonView.RPC(nameof(SetLayer), RpcTarget.AllBuffered, "Default");
                    photonView.RPC(nameof(SetColliderTrigger), RpcTarget.AllBuffered, false);
                }
            });
        }
    }
}