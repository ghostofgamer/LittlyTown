using ItemContent;
using UnityEngine;

namespace ItemPositionContent
{
    public class ItemPosition : MonoBehaviour
    {
        [SerializeField] private ItemPosition _northPosition;
        [SerializeField] private ItemPosition _westPosition;
        [SerializeField] private ItemPosition _eastPosition;
        [SerializeField] private ItemPosition _southPosition;
        [SerializeField] private PositionMatcher _positionMatcher;
        [SerializeField] private bool _isSelected = false;
        [SerializeField] private bool _isBusy = false;
        [SerializeField] private ItemPosition _road;
        [SerializeField] private bool _isWater;
        [SerializeField] private bool _isElevation;
        [SerializeField] private Transform _container;
        
        private Item _item;
        private ItemPosition[] _itemPositions;

        public ItemPosition[] ItemPositions => _itemPositions;

        public Item Item => _item;

        public bool IsBusy => _isBusy;

        public bool IsSelected => _isSelected;

        public bool IsWater => _isWater;

        public bool IsElevation => _isElevation;

        public ItemPosition SouthPosition => _southPosition;

        public ItemPosition NorthPosition => _northPosition;

        public ItemPosition EastPosition => _eastPosition;

        public ItemPosition WestPosition => _westPosition;

        private void Start()
        {
            // _itemPositions = new ItemPosition[4] {_northPosition, _westPosition, _eastPosition, _southPosition};
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Item item) && item.IsActive)
            {
                _item = item;
                _isBusy = true;
            }
        }

        public void SetRoad(ItemPosition itemPosition)
        {
            if (_road != null)
                _road.gameObject.SetActive(false);

            _road = itemPosition;
        }

        public void DeliverObject(Item item)
        {
            _item = item;
            _positionMatcher.LookAround(this);
        }

        public void SetSelected()
        {
            _isSelected = true;
        }

        public void ClearingPosition()
        {
            if (_item == null)
                return;
            
            _item = null;
            _isBusy = false;
            _isSelected = false;
        }

        public void SetNeighbors(ItemPosition north, ItemPosition west, ItemPosition east, ItemPosition south)
        {
            _northPosition = north;
            _westPosition = west;
            _eastPosition = east;
            _southPosition = south;
            _itemPositions = new ItemPosition[4] {_northPosition, _westPosition, _eastPosition, _southPosition};
        }
    }
}