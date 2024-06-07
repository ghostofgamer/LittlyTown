using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dragger;
using ItemContent;
using ItemPositionContent;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _environment;
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
    [SerializeField] private Initializator _initializator;
    
    [SerializeField] private Item _prefabItem;
    [SerializeField] private ItemPosition[] _positions;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private PositionMatcher _positionMatcher;
    [SerializeField] private MoveCounter _moveCounter;
    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private LookMerger _lookMerger;
    [SerializeField] private Transform _container;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.165f);
    private Coroutine _coroutine;
    private ItemPosition _position;

    
    
    public event Action ItemCreated;

    public event Action PositionsFilled;

    public event Action<ItemPosition, Item> LooksNeighbors;


private List<ItemPosition > _itemPos = new List<ItemPosition>();
    private int _index;
    
    private void Start()
    {
        // OnCreateItem();
    }

    private void OnEnable()
    {
        _positionMatcher.NotMerged += OnCreateItem;
        _lookMerger.NotMerged += OnCreateItem;
        _chooseMap.MapChanged += SetIndex;
    }

    private void OnDisable()
    {
        _positionMatcher.NotMerged -= OnCreateItem;
        _lookMerger.NotMerged -= OnCreateItem;
        _chooseMap.MapChanged -= SetIndex;
    }

    public void OnCreateItem()
    {
        if (!_moveCounter.IsThereMoves || _itemDragger.SelectedObject != null || _itemDragger.TemporaryItem != null)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(CreateNewItem());
    }

    public ItemPosition GetPosition()
    {
        // _visualItemsDeactivator.SetPositions(_initializator.ItemPositions);
        List<ItemPosition> freePositions = _initializator.ItemPositions
            .Where(p => !p.GetComponent<ItemPosition>().IsBusy && !p.GetComponent<ItemPosition>().IsWater).ToList();
        int randomIndex = Random.Range(0, freePositions.Count);

        if (freePositions.Count <= 0)
        {
            PositionsFilled?.Invoke();
            return null;
        }

        ItemPosition randomFreePosition = freePositions[randomIndex];
        return randomFreePosition;
    }

    /*public ItemPosition GetPosition()
    {
        List<ItemPosition> freePositions = _positions
            .Where(p => !p.GetComponent<ItemPosition>().IsBusy && !p.GetComponent<ItemPosition>().IsWater).ToList();
        int randomIndex = Random.Range(0, freePositions.Count);

        if (freePositions.Count <= 0)
        {
            PositionsFilled?.Invoke();
            return null;
        }

        ItemPosition randomFreePosition = freePositions[randomIndex];
        return randomFreePosition;
    }*/
    
    public void SetPositions()
    {
        ItemPosition[] itemPositions = _environment[_index].GetComponentsInChildren<ItemPosition>(true);

        foreach (var itemPosition in itemPositions)
        {
            if (!itemPosition.enabled)
                continue;
            
            if (!_itemPos.Contains(itemPosition))
            {
                _itemPos.Add(itemPosition);
            }
        }
        
        _visualItemsDeactivator.SetPositions(_itemPos);
    }

    private void SetIndex(int index)
    {
        _index = index;                
    }

    private IEnumerator CreateNewItem()
    {
        yield return _waitForSeconds;

        _position = GetPosition();

        if (_position == null)
            yield break;

        // Item item = Instantiate(_prefabItem, _position.transform.position, Quaternion.identity);
        // Debug.Log("Spawner");
        Item item = Instantiate(_dropGenerator.GetItem(), _position.transform.position,
            Quaternion.identity,_initializator.CurrentMap.ItemsContainer);
        _itemDragger.SetItem(item, _position);
        ItemCreated?.Invoke();
        LooksNeighbors?.Invoke(_position, item);
    }
    
    /*private IEnumerator CreateNewItem()
    {
        yield return _waitForSeconds;

        _position = GetPosition();

        if (_position == null)
            yield break;

        // Item item = Instantiate(_prefabItem, _position.transform.position, Quaternion.identity);
        // Debug.Log("Spawner");
        Item item = Instantiate(_dropGenerator.GetItem(), _position.transform.position,
            Quaternion.identity,_container);
        _itemDragger.SetItem(item, _position);
        ItemCreated?.Invoke();
        LooksNeighbors?.Invoke(_position, item);
    }*/
}