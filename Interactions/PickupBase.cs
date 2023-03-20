using Photon.Pun;
using UnityEngine;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
    public class PickupBase : PlaceableObject, IPickup
    {
        public GameObject visuals;
        [HideInInspector] public bool highlighted;

        private void Awake()
        {
            if (visuals == null) 
                visuals = gameObject;
        }

        public virtual void Pickup(Transform tr)
        {
            
        }

        public virtual void Place(Transform tr)
        {
            
        }
    }
}