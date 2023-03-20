using Photon.Pun;
using UnityEngine;
using FMODUnity;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
    public class MovableObject : PickupBase, IInteractable
    {
        private Joint _joint;
        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public override void Pickup(Transform tr)
        {
            photonView.RPC(nameof(SetLayer), RpcTarget.AllBuffered, "Ignore Raycast");
            _joint = tr.parent.GetComponentInChildren<Joint>();
            _joint.connectedBody = _rigidbody;
            _rigidbody.drag = 50;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            RuntimeManager.PlayOneShot("event:/Interactables/Object Grab");
        }

        public override void Place(Transform tr)
        {
            photonView.RPC(nameof(SetLayer), RpcTarget.AllBuffered, "Default");
            _joint.connectedBody = null;
            _rigidbody.drag = 5;
            RuntimeManager.PlayOneShot("event:/Interactables/Object Let Go");
        }

        public void Interact()
        {
            _rigidbody.useGravity = !_rigidbody.useGravity;
            RuntimeManager.PlayOneShot("event:/Interactables/Object Gravity");
        }

        [PunRPC]
        public void SetLayer(string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}