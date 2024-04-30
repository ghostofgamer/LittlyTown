using UnityEngine;

public class ItemPosition : MonoBehaviour
{
    [SerializeField] private ItemPosition _northPosition;
    [SerializeField] private ItemPosition _westPosition;
    [SerializeField] private ItemPosition _eastPosition;
    [SerializeField] private ItemPosition _southPosition;

    [SerializeField]private bool _isBusy = false;

    private ItemPosition _road;
    
    public bool IsBusy => _isBusy;
    
    public ItemPosition SouthPosition=> _southPosition;
    public ItemPosition NorthPosition=> _northPosition;
    public ItemPosition EastPosition=> _eastPosition;
    public ItemPosition WestPosition=>_westPosition;

    private void Start()
    {
        
    }

    public void SetRoad(ItemPosition itemPosition)
    {
        if(_road!=null)
            _road.gameObject.SetActive(false);

        _road = itemPosition;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Item item))
            _isBusy = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item))
            _isBusy = false;
    }
}