using System.Collections.Generic;
using UnityEngine;

namespace ItemPositionContent
{
    public class VisualItemsDeactivator : MonoBehaviour
    {
        [SerializeField] private List<VisualItemPosition> _itemPositions;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private MoveCounter _moveCounter;

        private void OnEnable()
        {
            _spawner.PositionsFilled += OnDeactivationVisual;
            _moveCounter.MoveOver += OnDeactivationVisual;
        }

        private void OnDisable()
        {
            _spawner.PositionsFilled -= OnDeactivationVisual;
            _moveCounter.MoveOver -= OnDeactivationVisual;
        }

        public void OnDeactivationVisual()
        {
            foreach (var position in _itemPositions)
                position.DeactivateVisual();
        }
    }
}