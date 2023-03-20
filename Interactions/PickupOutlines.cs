using UnityEngine;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PlayerInteractions))]
    public class PickupOutlines : MonoBehaviour
    {
        [SerializeField] private Color outlineColor;

        private Outline _highlightedPickup;
        private bool _canHighlight = true;
        private PlayerInteractions _interactions;
        private Camera _camera;
        private LayerMask _layerMask;
        private float _distanceLimit;

        private void Start()
        {
            _camera = Camera.main;
            _interactions = GetComponent<PlayerInteractions>();
            _distanceLimit = _interactions.PickupMaxDistance;
            _layerMask = _interactions.Mask;
        }

        private void Update()
        {
            if (Physics.Raycast(GetRay(), out RaycastHit hitInfo, _distanceLimit, _layerMask))
                SetOutline(hitInfo);
            else
                RemoveOutline();
        }

        private void SetOutline(RaycastHit hitInfo)
        {
            var pickup = hitInfo.collider.GetComponent<PickupBase>();
            if (pickup == null)
            {
                RemoveOutline();
                return;
            }
            if (!_canHighlight) return;

            _canHighlight = false;
            if (pickup.visuals.GetComponent<Outline>() != null) return;
            Outline outline = pickup.visuals.AddComponent<Outline>();
            outline.OutlineColor = outlineColor;
            outline.OutlineWidth = 16;
            _highlightedPickup = outline;
        }

        private void RemoveOutline()
        {
            if (_highlightedPickup != null)
                Destroy(_highlightedPickup);

            _canHighlight = true;
        }

        protected Ray GetRay()
        {
            return new Ray(_camera.transform.position, _camera.transform.forward);
        }
    }
}