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

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.165f);
    private Coroutine _coroutine;

    public event Action ItemCreated;

    private void Start()
    {
        // OnCreateItem();
    }

    private void OnEnable()
    {
        _positionMatcher.NotMerged += OnCreateItem;
    }

    private void OnDisable()
    {
        _positionMatcher.NotMerged -= OnCreateItem;
    }

    public void OnCreateItem()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(CreateNewItem());
    }

    private ItemPosition GetPosition()
    {
        List<ItemPosition> freePositions = _positions.Where(p => !p.GetComponent<ItemPosition>().IsBusy).ToList();
        int randomIndex = Random.Range(0, freePositions.Count);
        ItemPosition randomFreePosition = freePositions[randomIndex];

        if (randomFreePosition == null)
        {
            Debug.Log("места кончились");
            return null;
        }

        return randomFreePosition;
    }

    private IEnumerator CreateNewItem()
    {
        yield return _waitForSeconds;
        ItemPosition position = GetPosition();

        if (position == null)
            yield break;

        Item item = Instantiate(_prefabItem, position.transform.position, Quaternion.identity);
        _itemDragger.SetItem(item, position);
        ItemCreated?.Invoke();
    }
}