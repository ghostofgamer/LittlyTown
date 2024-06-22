using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using UnityEngine;

public class LightHouseKeeper : MonoBehaviour
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private RemovalItems _removalItems;
    [SerializeField] private ReplacementPosition _replacementPosition;
    [SerializeField] private Merger _merger;
    [SerializeField] private ItemThrower _itemThrower;
    
    private List<LightHouseTrigger> _lightHouses = new List<LightHouseTrigger>();
    private Coroutine _coroutine;

    public event Action CheckCompleted;

    private void OnEnable()
    {
        // _itemDragger.BuildItem += AddMayak;
        _itemThrower.BuildItem += AddMayak;
        _itemThrower.PlaceChanged += CheckHousesAround;
        _removalItems.ItemRemoved += RemoveMayak;
        _replacementPosition.PositionsChanged += CheckHousesAround;
        _merger.Mergered += CheckHousesAround;
    }

    private void OnDisable()
    {
        // _itemDragger.BuildItem -= AddMayak;
        _itemThrower.BuildItem -= AddMayak;
        _itemThrower.PlaceChanged -= CheckHousesAround;
        _removalItems.ItemRemoved -= RemoveMayak;
        _replacementPosition.PositionsChanged -= CheckHousesAround;
        _merger.Mergered -= CheckHousesAround;
    }

    private void AddMayak(Item item)
    {
        LightHouseTrigger lightHouseTrigger = item.GetComponent<LightHouseTrigger>();

        if (lightHouseTrigger != null)
        {
            _lightHouses.Add(lightHouseTrigger);
        }

        Show();
    }

    private void RemoveMayak(Item item)
    {
        LightHouseTrigger lightHouseTrigger = item.GetComponent<LightHouseTrigger>();

        Debug.Log("Удаляем маяк и очишает доход ");

        if (lightHouseTrigger != null)
        {
            lightHouseTrigger.RemoveHouses();
            _lightHouses.Remove(lightHouseTrigger);
        }

        Show();
    }

    private void CheckHousesAround()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        yield return new WaitForSeconds(0.165f);

        foreach (var lightHouse in _lightHouses)
        {
            lightHouse.Look();
        }

        yield return null;
        CheckCompleted?.Invoke();
    }

    private void Show()
    {
        // Debug.Log("колличество маяков " + _lightHouses.Count);
    }
}