using UnityEngine;

namespace Team11.Interactions
{
    public class PolarizerInteractions : PlayerInteractions
    {
        protected override void Place()
        {
            Drop();
        }

        protected override void Pick()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, pickupMaxDistance, layerMask))
            {
                var pickup = hitInfo.collider.GetComponent<MovableObject>();
                if (pickup == null) return;

                pickup.Pickup(pickupHolder);
                _currentPickup = pickup;
                _pickedUp = true;
                _pickOrigin = hitInfo.point;
            }
        }
    }
}