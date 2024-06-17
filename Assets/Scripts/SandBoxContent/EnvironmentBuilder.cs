using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnvironmentBuilder : MonoBehaviour
{
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private ItemPosition _tileWater;
    [SerializeField] private ItemPosition _tileClear;
    [SerializeField] private ItemPosition _tileElevation;
    [SerializeField] private Transform _container;
    [SerializeField] private ItemPosition[] _itemPositions;

    private Item _item;
    private int _layerMask;
    private int _layer = 3;

    private bool _isTileClear;
    private bool _isTileWater;
    private bool _isTileElevation;
    private bool _isTileTrail;
    private bool _isTileRoad;

    private void Start()
    {
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;

        foreach (var itemPosition in _itemPositions)
        {
            itemPosition.GetComponent<FinderPositions>().FindNeighbor();
        }
    }

    private void DeactivateEnvironment()
    {
        _isTileClear = false;
        _isTileWater = false;
        _isTileElevation = false;
        _isTileTrail = false;
        _isTileRoad = false;
    }

    public void ChangeEnvironment(Environments environmentName)
    {
        string name = environmentName.ToString();
        DeactivateEnvironment();

        switch (name)
        {
            case "ClearTile":
                _isTileClear = true;
                break;
            case "Water":
                _isTileWater = true;
                break;
            case "Elevation":
                _isTileElevation = true;
                break;
            case "Trail":
                _isTileTrail = true;
                break;
            case "Road":
                _isTileRoad = true;
                break;
        }
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("ГГГ");
                return;
            }
        }

        if (_isTileClear)
        {
            Debug.Log("вфходм");
            
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (itemPosition.IsBusy)
                        {
                            itemPosition.Item.gameObject.SetActive(false);
                            itemPosition.ClearingPosition();
                            return;
                        }

                        ItemPosition itemPositionTile = Instantiate(_tileClear, itemPosition.transform.position,
                            Quaternion.identity, _container);
                        itemPosition.SetRoad(itemPositionTile);
                        itemPosition.DeactivationAll();
                        _roadGenerator.GenerateSandBoxTrail(_itemPositions, _container);
                    }
                }
            }
        }
        
        if (_isTileWater)
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (itemPosition.IsBusy)
                        {
                            itemPosition.Item.gameObject.SetActive(false);
                            itemPosition.ClearingPosition();
                            return;
                        }
                        
                        ItemPosition itemPositionTile = Instantiate(_tileWater, itemPosition.transform.position,
                            Quaternion.identity, _container);
                        itemPosition.SetRoad(itemPositionTile);
                        itemPosition.DeactivationAll();
                        itemPosition.ActivationWater();
                        _roadGenerator.GenerateSandBoxTrail(_itemPositions, _container);
                    }
                }
            }
        }

        if (_isTileTrail)
        {
            if (Input.GetMouseButton(0))
            {
                /*
                                RaycastHit hit;
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);*/


                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (itemPosition.IsTrail)
                            return;


                        if (itemPosition.IsBusy)
                        {
                            itemPosition.Item.gameObject.SetActive(false);
                            itemPosition.ClearingPosition();
                            return;
                        }
                        
                        itemPosition.DeactivationAll();
                        itemPosition.ActivateTrail();
                        _roadGenerator.GenerateSandBoxTrail(_itemPositions, _container);
                    }
                }
            }
        }
        
        if (_isTileRoad)
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (itemPosition.IsRoad)
                            return;

                        if (itemPosition.IsBusy)
                        {
                            itemPosition.Item.gameObject.SetActive(false);
                            itemPosition.ClearingPosition();
                            return;
                        }
                        
                        
                        itemPosition.DeactivationAll();
                        itemPosition.EnableRoad();
                        _roadGenerator.GenerateSandBoxTrail(_itemPositions, _container);
                    }
                }
            }
        }
    }
}