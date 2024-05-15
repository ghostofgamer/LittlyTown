using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Item _prefabItem;
    [SerializeField] private ItemPosition[] _positions;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private PositionMatcher _positionMatcher;
    [SerializeField] private MoveCounter _moveCounter;
    [SerializeField] private DropGenerator _dropGenerator;
[SerializeField]private LookMerger _lookMerger;
    
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.165f);
    private Coroutine _coroutine;
    private ItemPosition _position;

    public event Action ItemCreated;

    public event Action PositionsFilled;

    public event Action<ItemPosition, Item> LooksNeighbors;

    private void Start()
    {
        // OnCreateItem();
    }

    private void OnEnable()
    {
        _positionMatcher.NotMerged += OnCreateItem;
        _lookMerger.NotMerged += OnCreateItem;
    }

    private void OnDisable()
    {
        _positionMatcher.NotMerged -= OnCreateItem;
        _lookMerger.NotMerged -= OnCreateItem;
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
            Quaternion.identity);
        _itemDragger.SetItem(item, _position);
        ItemCreated?.Invoke();
        LooksNeighbors?.Invoke(_position, item);
    }
}