using System.Collections.Generic;
using CountersContent;
using SpawnContent;
using UnityEngine;

namespace ItemPositionContent
{
    public class VisualItemsDeactivator : MonoBehaviour
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField] private MoveCounter _moveCounter;

        private List<VisualItemPosition> _itemPositions = new List<VisualItemPosition>();

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

        public void SetPositions(List<ItemPosition> itemPositions)
        {
            _itemPositions = new List<VisualItemPosition>();

            foreach (var position in itemPositions)
            {
                if (position.GetComponent<VisualItemPosition>())
                    _itemPositions.Add(position.GetComponent<VisualItemPosition>());
            }
        }
    }
}