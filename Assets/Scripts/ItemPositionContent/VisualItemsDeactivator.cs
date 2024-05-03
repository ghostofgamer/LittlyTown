using System.Collections.Generic;
using UnityEngine;

namespace ItemPositionContent
{
    public class VisualItemsDeactivator : MonoBehaviour
    {
        [SerializeField] private List<VisualItemPosition> _itemPositions;

        public void DeactivationVisual()
        {
            foreach (var position in _itemPositions)
                position.DeactivateVisual();
        }
    }
}