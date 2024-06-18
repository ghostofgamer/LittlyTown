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
        [SerializeField] private LookMerger _lookMerger;
        [SerializeField] private ItemPosition _waterTile;
        [SerializeField] private ItemPosition[] _roadPositions;


        private Item _item;
        private ItemPosition[] _itemPositions;


        public bool IsRoad { get; private set; } = false;

        public bool IsTrail { get; private set; } = false;

        public ItemPosition[] RoadPositions => _roadPositions;

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

        public bool IsReplaceSelected { get; private set; } = false;

        private void Start()
        {
            _itemPositions = new ItemPosition[4] {_northPosition, _westPosition, _eastPosition, _southPosition};
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Item item) && item.IsActive)
            {
                _item = item;
                _isBusy = true;
            }
        }

        public void EnableRoad()
        {
            IsRoad = true;
        }

        public void DisableRoad()
        {
            IsRoad = false;
        }

        public void SetRoad(ItemPosition itemPosition)
        {
            if (_road != null)
                _road.gameObject.SetActive(false);

            _road = itemPosition;
            // Debug.Log("тут");
        }

        public void SetFirstRoad(ItemPosition itemPosition)
        {
            // Debug.Log("там");
            if (IsWater)
                return;

            itemPosition.gameObject.SetActive(true);
            _road = itemPosition;
        }

        public void DeliverObject(Item item)
        {
            _item = item;
            if (IsRoad)
                IsRoad = false;
            // _positionMatcher.LookAround(this);
            _lookMerger.LookAround(this, _item);
        }

        public void SetSelected()
        {
            _isSelected = true;
        }

        public void DeactivationSelected()
        {
            _isSelected = false;
        }

        public void ClearingPosition()
        {
            if (_item == null)
                return;

            _item = null;
            _isBusy = false;

            if (!IsReplaceSelected)
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

        public void SetPositions()
        {
            _itemPositions = new ItemPosition[4] {_northPosition, _westPosition, _eastPosition, _southPosition};
        }

        public void ReplaceSelectedActivate()
        {
            IsReplaceSelected = true;
        }

        public void ReplaceSelectedDeactivate()
        {
            IsReplaceSelected = false;
        }

        public void SetWater()
        {
            // Debug.Log(this.name);
            _road.gameObject.SetActive(false);
            _waterTile.gameObject.SetActive(true);
            _isWater = true;
        }

        public void ResetWater()
        {
            if (_waterTile == null)
                return;

            _road.gameObject.SetActive(true);
            _waterTile.gameObject.SetActive(false);
            _isWater = false;
        }

        public void ActivateTrail()
        {
            IsTrail = true;
        }

        public void DeactivateTrail()
        {
            IsTrail = false;
        }

        public void ActivationWater()
        {
            _isWater = true;
        }

        public void DeactivationWater()
        {
            _isWater = false;
        }

        public void DeactivationAll()
        {
            _isElevation = false;
            _isWater = false;
            IsRoad = false;
            IsTrail = false;
        }

        public void OnElevation()
        {
            _isElevation = true;
        }

        public void OnBusy()
        {
            _isBusy = true;
        }

        public void Init(bool isBusy, bool isElevation, bool isWater, bool isRoad, bool isTrail)
        {
            _isBusy = isBusy;
            _isElevation = isElevation;
            _isWater = isWater;
            IsRoad = isRoad;
            IsTrail = isTrail;
        }
    }
}