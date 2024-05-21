using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Territory[] _territorys;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Item[] _items;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private FinderPositions[] _finderPositions;
    [SerializeField] private Spawner _spawner;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);
    private WaitForSeconds _waitForSecondsTri = new WaitForSeconds(0.3f);
    private WaitForSeconds _waitForSecondsMoment = new WaitForSeconds(0.1f);
    private List<ItemPosition> _clearPositions;
    private int _randomIndex;

    public event Action GenerationCompleted;

    /*
    private void Start()
    {
        Generation();
    }
    */

    public void Generation()
    {
        foreach (var territory in _territorys)
        {
            // territory.PositionDeactivation();
            territory.gameObject.SetActive(false);
            _roadGenerator.DeactivateRoad();
        }

        StartCoroutine(StartGenerationTerritory());
    }

    private IEnumerator StartGenerationTerritory()
    {
        foreach (var territory in _territorys)
        {
            territory.gameObject.SetActive(true);
            territory.PositionActivation();
            yield return _waitForSecondsTri;
        }

        yield return null;
        /*yield return _waitForSeconds;*/

        /*foreach (var item in _items)
        {
            _clearPositions = new List<ItemPosition>();
            _clearPositions = _itemPositions.Where(p => !p.GetComponent<ItemPosition>().IsBusy).ToList();
            _randomIndex = Random.Range(0, _clearPositions.Count);
            item.gameObject.SetActive(true);
            item.transform.position = _clearPositions[_randomIndex].transform.position;
            item.Activation();
            // _clearPositions[_randomIndex].DeliverObject(item);
            yield return _waitForSecondsMoment;
        }*/

        foreach (var finderPosition in _finderPositions)
            finderPosition.FindNeighbor();

        yield return _waitForSecondsMoment;
        _roadGenerator.OnGeneration();
        yield return _waitForSecondsMoment;
        _spawner.OnCreateItem();
        yield return _waitForSecondsTri;
        GenerationCompleted?.Invoke();
    }

    public void ShowFirstMap()
    {
        foreach (var territory in _territorys)
        {
            // territory.PositionDeactivation();
            territory.gameObject.SetActive(false);
            _roadGenerator.DeactivateRoad();
        }

        StartCoroutine(StartGenerationFirstMap());
    }

    public IEnumerator StartGenerationFirstMap()
    {
        foreach (var territory in _territorys)
        {
            territory.gameObject.SetActive(true);
            territory.PositionActivation();
            yield return _waitForSecondsTri;
        }

        yield return null;
        /*yield return _waitForSeconds;*/

        /*foreach (var item in _items)
        {
            _clearPositions = new List<ItemPosition>();
            _clearPositions = _itemPositions.Where(p => !p.GetComponent<ItemPosition>().IsBusy).ToList();
            _randomIndex = Random.Range(0, _clearPositions.Count);
            item.gameObject.SetActive(true);
            item.transform.position = _clearPositions[_randomIndex].transform.position;
            item.Activation();
            // _clearPositions[_randomIndex].DeliverObject(item);
            yield return _waitForSecondsMoment;
        }*/

        foreach (var finderPosition in _finderPositions)
            finderPosition.FindNeighbor();

        yield return _waitForSecondsMoment;
        _roadGenerator.OnGeneration();
        yield return _waitForSecondsMoment;
        /*_spawner.OnCreateItem();
        yield return _waitForSecondsTri;
        GenerationCompleted?.Invoke();*/
    }
    
    public void ShowMap()
    {
        foreach (var territory in _territorys)
        {
            territory.gameObject.SetActive(true);
            territory.ShowPositions();
        }

        /*foreach (var item in _items)
        {
            _clearPositions = new List<ItemPosition>();
            _clearPositions = _itemPositions
                .Where(p => !p.GetComponent<ItemPosition>().IsBusy && !p.GetComponent<ItemPosition>().IsWater).ToList();
            _randomIndex = Random.Range(0, _clearPositions.Count);
            item.gameObject.SetActive(true);
            item.transform.position = _clearPositions[_randomIndex].transform.position;
            item.Activation();
            // _clearPositions[_randomIndex].DeliverObject(item);
        }*/

        foreach (var finderPosition in _finderPositions)
            finderPosition.FindNeighbor();

        _roadGenerator.OnGeneration();
    }
}