using UnityEngine;
using System;
using Photon.Pun;

namespace Team11.Interactions
{
    public class LitomancerInteractions : PlayerInteractions
    {
        public event Action<bool> OnHaveTorch;

        [SerializeField] private bool canPickEverything;

        protected override void Place()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, placeMaxDistance, Mask))
            {
                var container = hitInfo.collider.GetComponent<IContainer>();
                if (container != null)
                {
                    if (container.PutIn(_currentPickup))
                    {
                        _currentPickup = null;
                        _pickedUp = false;
                        OnHaveTorch?.Invoke(false);
                        return;
                    }
                }
            }

            Drop();
        }

        protected override void Drop()
        {
            base.Drop();
            OnHaveTorch?.Invoke(false);
        }

        protected override void Pick()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, PickupMaxDistance, Mask))
            {
                PickupBase pickup = canPickEverything
                    ? hitInfo.collider.GetComponent<PickupBase>()
                    : hitInfo.collider.GetComponent<Torch>();
                if (pickup == null) return;
                pickup.photonView.RequestOwnership();

                pickup.Pickup(pickupHolder);
                _currentPickup = pickup;
                _pickedUp = true;
                _pickOrigin = hitInfo.point;
                if (!spaceOrTime) timeSpent = 0;

                if (pickup is Torch) OnHaveTorch?.Invoke(true);
            }
        }
    }
}