using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemContent;
using ItemPositionContent;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Territory[] _territorys;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Item[] _items;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private FinderPositions[] _finderPositions;
    [SerializeField] private Spawner _spawner;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);
    private List<ItemPosition> _clearPositions;
    private int _randomIndex;

    private void Start()
    {
    }

    public void Generation()
    {
        foreach (var territory in _territorys)
            territory.gameObject.SetActive(false);

        StartCoroutine(StartGenerationTerritory());
    }

    private IEnumerator StartGenerationTerritory()
    {
        foreach (var territory in _territorys)
        {
            territory.gameObject.SetActive(true);
            territory.PositionActivation();
            yield return _waitForSeconds;
        }

        yield return _waitForSeconds;

        /*foreach (var item in _items)
        {
            _clearPositions = new List<ItemPosition>();
            _clearPositions = _itemPositions.Where(p => !p.GetComponent<ItemPosition>().IsBusy).ToList();
            _randomIndex = Random.Range(0, _clearPositions.Count);
            item.gameObject.SetActive(true);
            item.transform.position = _clearPositions[_randomIndex].transform.position;
            item.Activation();
            // _clearPositions[_randomIndex].DeliverObject(item);
            yield return _waitForSeconds;
        }

        yield return _waitForSeconds;*/

        foreach (var finderPosition in _finderPositions)
            finderPosition.FindNeighbor();

        yield return _waitForSeconds;
        _roadGenerator.OnGeneration();
        yield return _waitForSeconds;
        _spawner.OnCreateItem();
    }
}