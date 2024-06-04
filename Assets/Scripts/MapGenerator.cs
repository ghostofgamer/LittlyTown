using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemContent;
using ItemPositionContent;
using MapsContent;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] _environments;

    private List<Territory> _targetTerritories = new List<Territory>();
    private List<FinderPositions> _targetFinderPositions = new List<FinderPositions>();
    private List<ItemPosition> _targetItemPositions = new List<ItemPosition>();

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

    private void Start()
    {
        foreach (var environment in _environments)
        {
            _targetTerritories = new List<Territory>();
            _targetFinderPositions = new List<FinderPositions>();
            _targetItemPositions = new List<ItemPosition>();

            Territory[] territories = environment.GetComponentsInChildren<Territory>(true);

            foreach (var territory in territories)
            {
                if (!_targetTerritories.Contains(territory))
                {
                    _targetTerritories.Add(territory);
                }
            }

            FinderPositions[] finderPositionScripts = environment.GetComponentsInChildren<FinderPositions>(true);

            foreach (FinderPositions finderPositionScript in finderPositionScripts)
            {
                if (!_targetFinderPositions.Contains(finderPositionScript))
                {
                    _targetFinderPositions.Add(finderPositionScript);
                }
            }

            ItemPosition[] itemPositions = environment.GetComponentsInChildren<ItemPosition>(true);

            foreach (var itemPosition in itemPositions)
            {
                if (!itemPosition.enabled)
                    continue;

                if (!_targetItemPositions.Contains(itemPosition))
                {
                    _targetItemPositions.Add(itemPosition);
                }
            }

            foreach (var territory in _targetTerritories)
            {
                // territory.PositionDeactivation();

                territory.gameObject.SetActive(false);

                // _roadGenerator.DeactivateRoad();
            }

            TestShowMap(_targetTerritories, _targetFinderPositions, environment.GetComponent<Map>().RoadsContainer, _targetItemPositions);
        }


        // _roadGenerator.TestGeneration(_targetItemPositions);

        // StartCoroutine(StartTestGenerationFirstMap(_targetTerritories,_targetFinderPositions));
    }
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

    public IEnumerator StartTestGenerationFirstMap(List<Territory> territories, List<FinderPositions> finderPositions)
    {
        foreach (var territory in territories)
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

        foreach (var finderPosition in finderPositions)
            finderPosition.FindNeighbor();

        yield return _waitForSecondsMoment;
        // _roadGenerator.OnGeneration();
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

    public void TestShowMap(List<Territory> territories, List<FinderPositions> finderPositions, Transform container,
        List<ItemPosition> itemPositions)
    {
        foreach (var territory in territories)
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

        foreach (var finderPosition in finderPositions)
            finderPosition.FindNeighbor();

        // _roadGenerator.TestGeneration(itemPositions, container);
        _roadGenerator.TestCreateRoadOneMoment(itemPositions, container);
    }


    public void TestGeneration(List<Territory> territories, List<FinderPositions> finderPositions)
    {
        foreach (var territory in territories)
        {
            // territory.PositionDeactivation();
            territory.gameObject.SetActive(false);
            _roadGenerator.DeactivateRoad();
        }

        StartCoroutine(StartTestGenerationTerritory(territories, finderPositions));
    }

    private IEnumerator StartTestGenerationTerritory(List<Territory> territories, List<FinderPositions> finderPositions)
    {
        foreach (var territory in territories)
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

        foreach (var finderPosition in finderPositions)
            finderPosition.FindNeighbor();

        /*yield return _waitForSecondsMoment;
        _roadGenerator.OnGeneration();
        yield return _waitForSecondsMoment;
        _spawner.OnCreateItem();
        yield return _waitForSecondsTri;
        GenerationCompleted?.Invoke();*/
    }
}