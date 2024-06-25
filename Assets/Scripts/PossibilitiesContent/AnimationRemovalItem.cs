using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace PossibilitiesContent
{
    public class AnimationRemovalItem : MonoBehaviour
    {
        private RaycastHit _hit;
        private Ray _ray;
        private int _layerMask;
        private int _layer = 3;

        private void Start()
        {
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
        }

        public void ActivateAnimation()
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                if (_hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy && !itemPosition.IsWater)
                    {
                        itemPosition.Item.GetComponent<ItemAnimation>().BusyPositionAnimation();
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                    else
                    {
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                }
            }
        }
    }
}