using UnityEngine;

namespace ItemPositionContent
{
    public class VisualItemPosition : MonoBehaviour
    {
        [SerializeField] private GameObject _visualPosition;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
    
        public void ActivateVisual()
        {
            _visualItemsDeactivator.DeactivationVisual();
            _visualPosition.SetActive(true);
        }
        
        public void DeactivateVisual()
        {
            _visualPosition.SetActive(false);
        }
    }
}
