using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Item _prefabItem;
    [SerializeField] private ItemPosition[] _positions;
    [SerializeField] private ItemDragger _itemDragger;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _itemDragger.PlaceChanged += CreateItem;
    }

    private void OnDisable()
    {
        _itemDragger.PlaceChanged -= CreateItem;
    }

    public void CreateItem()
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
            return null;

        return randomFreePosition;
    }

    private IEnumerator CreateNewItem()
    {
        yield return _waitForSeconds;
        ItemPosition position = GetPosition();

        if (position == null)
            yield break;

        Item item = Instantiate(_prefabItem, position.transform.position, Quaternion.identity);
        _itemDragger.SetItem(item);
    }
}