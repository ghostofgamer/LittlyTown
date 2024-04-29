using UnityEngine;

public class ItemPosition : MonoBehaviour
{
    [SerializeField] private ItemPosition _northPosition;
    [SerializeField] private ItemPosition _westPosition;
    [SerializeField] private ItemPosition _eastPosition;
    [SerializeField] private ItemPosition _southPosition;

    private bool _isBusy = false;

    public bool IsBusy => _isBusy;
    
    public ItemPosition SouthPosition=> _southPosition;
    public ItemPosition NorthPosition=> _northPosition;
    public ItemPosition EastPosition=> _eastPosition;
    public ItemPosition WestPosition=>_westPosition;

    private void Start()
    {
        
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