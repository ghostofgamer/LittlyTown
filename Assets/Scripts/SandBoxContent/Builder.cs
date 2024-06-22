using System.Collections;
using ItemPositionContent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Builder : MonoBehaviour
{
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private ItemPosition _clearTile;
    [SerializeField] private ItemPosition _tileWater;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _roadContainer;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private BuildButton _buildButton;

    private int _layerMask;
    private int _layer = 3;
    private ItemPosition _lastItemPosition;
    private ItemPosition _currentItemPosition;
    private bool _isFirstOpen = true;

    protected ItemPosition[] ItemPositions => _itemPositions;

    protected Transform Container => _container;

    protected Transform RoadContainer => _roadContainer;

    protected ItemPosition ClearTile => _clearTile;

    protected ItemPosition TileWater => _tileWater;

    protected virtual void Start()
    {
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;
        _scrollbar.value = 0;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (_currentItemPosition != itemPosition)
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();

                    _currentItemPosition = itemPosition;
                    _lastItemPosition = itemPosition;
                    TakeAction(itemPosition);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_lastItemPosition != null)
                _lastItemPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
        }
    }

    protected void StartRoadGeneration()
    {
        _roadGenerator.GenerateSandBoxTrail(_itemPositions, _roadContainer);
    }

    protected abstract void TakeAction(ItemPosition itemPosition);

    protected abstract void FirstChoose();

    public void Open()
    {
        if (!_isFirstOpen)
            return;

        _isFirstOpen = false;
        _buildButton.Activate();
        FirstChoose();
    }
}