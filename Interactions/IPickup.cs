using UnityEngine;

namespace Team11.Interactions
{
    public interface IPickup
    {
        public void Pickup(Transform tr);
        public void Place(Transform tr);
    }
}