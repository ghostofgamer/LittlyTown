using UnityEngine;

public class ItemPosition : MonoBehaviour
{
    [SerializeField] private ItemPosition _northPosition;
    [SerializeField] private ItemPosition _westPosition;
    [SerializeField] private ItemPosition _eastPosition;
    [SerializeField] private ItemPosition _southPosition;
    [SerializeField] private PositionMatcher _positionMatcher;
    [SerializeField] private Merge _merge;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private GameObject _visualPosition;
    [SerializeField] private ItemPosition[] _allPositions;
    [SerializeField] private bool _isBusy = false;

    private ItemPosition _road;
    private Item _item;
    private ItemPosition[] _itemPositions;

    public ItemPosition[] ItemPositions => _itemPositions;

    public Item Item => _item;

    public bool IsBusy => _isBusy;

    public bool IsSelected=> _isSelected;
    
    public ItemPosition SouthPosition => _southPosition;

    public ItemPosition NorthPosition => _northPosition;

    public ItemPosition EastPosition => _eastPosition;

    public ItemPosition WestPosition => _westPosition;

    private void Start()
    {
        _itemPositions = new ItemPosition[4] {_northPosition, _westPosition, _eastPosition, _southPosition};
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
        // _merge.SetPosition(this);
        _positionMatcher.TryMerge(this);
        // Debug.Log("PosItem");
        // _merge.Testmerge();
    }

    public void SetSelected(Item item)
    {
        // _item = item;
        _isSelected = true;
    }

    public void SetValue()
    {
       
    }
    
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("наступили");
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Item item) && item.IsActive)
        {
            _item = item;
            _isBusy = true;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item) && item.IsActive)
        {
            Debug.Log("Ушел" + this.name);
            _isBusy = false;
            _item = null;
        }
    }*/

    public void ClearingItem()
    {
        if (_item == null)
            return;

        _item = null;
        _isBusy = false;
        _isSelected = false;
    }

    public void ActivateVisual()
    {
        foreach (var position in _allPositions)
        {
            position._visualPosition.SetActive(false);
        }

        _visualPosition.SetActive(true);
    }
}