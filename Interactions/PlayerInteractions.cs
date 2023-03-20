using System;
using UnityEngine;
using Photon.Pun;

namespace Team11.Interactions
{
    public class PlayerInteractions : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected float pickupMaxDistance = 6f;
        [SerializeField] protected float placeMaxDistance = 6f;
        [SerializeField] protected float distanceThreshold = 20;
        [SerializeField] protected bool spaceOrTime;
        [SerializeField] protected float timeThreshold = 5f;
        [SerializeField] protected float timeSpent;

        protected Transform pickupHolder;
        protected Camera cam;
        protected Vector3 _pickOrigin;
        protected bool _pickedUp;
        protected IPickup _currentPickup;

        public float PickupMaxDistance => pickupMaxDistance;
        public LayerMask Mask => layerMask;

        private void Start()
        {
            cam = Camera.main;
            pickupHolder = transform.GetChild(0).GetChild(0);
        }

        protected void OnInteract()
        {
            if (_pickedUp)
                Place();
            else
                Pick();
        }

        protected void OnPress()
        {
            Press();
        }

        protected void Update()
        {
            if (_pickedUp)
            {
                if (spaceOrTime)
                {
                    if (Vector3.Distance(pickupHolder.position, _pickOrigin) >= distanceThreshold)
                        Drop();
                }
                else
                {
                    timeSpent += Time.deltaTime;
                    if (timeSpent >= timeThreshold)
                        Drop();
                }
            }
        }

        private void Press()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, placeMaxDistance, layerMask))
            {
                var interactable = hitInfo.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    hitInfo.collider.GetComponent<PhotonView>().RequestOwnership();
                    interactable.Interact();
                }
            }
        }

        protected virtual void Place()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, placeMaxDistance, layerMask))
            {
                var container = hitInfo.collider.GetComponent<IContainer>();
                if (container != null)
                {
                    if (container.PutIn(_currentPickup))
                    {
                        _currentPickup = null;
                        _pickedUp = false;
                        return;
                    }
                }
            }

            Drop();
        }

        protected virtual void Drop()
        {
            _currentPickup.Place(null);
            _currentPickup = null;
            _pickedUp = false;
        }

        protected virtual void Pick()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, pickupMaxDistance, layerMask))
            {
                var pickup = hitInfo.collider.GetComponent<IPickup>();
                if (pickup == null) return;

                pickup.Pickup(pickupHolder);
                _currentPickup = pickup;
                _pickedUp = true;
                _pickOrigin = hitInfo.point;
            }
        }

        protected Ray GetRay()
        {
            return new Ray(cam.transform.position, cam.transform.forward);
        }

        public void SetCamera(Camera cam)
        {
            this.cam = cam;
            pickupHolder = cam.transform.GetChild(0);
        }

        protected void OnDrawGizmos()
        {
            if (cam == null) return;
            Gizmos.color = Color.red;
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, pickupMaxDistance, layerMask))
            {
                Gizmos.DrawLine(ray.origin, hitInfo.point);
            }

            if (_pickedUp)
                Gizmos.DrawLine(pickupHolder.position, _pickOrigin);
        }
    }
}