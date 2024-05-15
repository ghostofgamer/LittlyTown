using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using UnityEngine;

public class LightHouseKeeper : MonoBehaviour
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField]private RemovalItems _removalItems;
    [SerializeField]private ReplacementPosition _replacementPosition;

    private List<LightHouseTrigger> _lightHouses = new List<LightHouseTrigger>();

    private void OnEnable()
    {
        _itemDragger.BuildItem += AddMayak;
        _itemDragger.PlaceChanged += CheckHousesAround;
        _removalItems.ItemRemoved += RemoveMayak;
        _replacementPosition.PositionsChanged += CheckHousesAround;
    }

    private void OnDisable()
    {
        _itemDragger.BuildItem -= AddMayak;
        _itemDragger.PlaceChanged -= CheckHousesAround;
        _removalItems.ItemRemoved -= RemoveMayak; 
        _replacementPosition.PositionsChanged -= CheckHousesAround;
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

        if (lightHouseTrigger != null)
            _lightHouses.Remove(lightHouseTrigger);

        Show();
    }

    private void CheckHousesAround()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        yield return  new WaitForSeconds(0.165f);
        
        foreach (var lightHouse in _lightHouses)
        {
            lightHouse.Look();
        }
    }

    private void Show()
    {
        Debug.Log("колличество маяков " + _lightHouses.Count);
    }
}