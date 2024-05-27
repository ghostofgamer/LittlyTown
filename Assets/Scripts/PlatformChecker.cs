using System.Collections.Generic;
using ItemContent;
using UnityEngine;

public class PlatformChecker : MonoBehaviour
{
    [SerializeField] private CollectionScreen _collectionScreen;
    [SerializeField] private GameObject[] _contentsCollections;
    [SerializeField] private CollectionMovement[] _collectionMovements;
    [SerializeField] private List<Item> _allCollectionItemsPC = new List<Item>();
    [SerializeField] private List<Item> _allCollectionItemsMobile = new List<Item>();
    
    private List<Item>[] _allCollectionItems = new List<Item>[2];

    private int _pcIndex = 0;
    private int _mobileIndex = 1;
    private int _currentIndex;

    private void Awake()
    {
        _allCollectionItems[_pcIndex] = _allCollectionItemsPC;
        _allCollectionItems[_mobileIndex] = _allCollectionItemsMobile;

        _currentIndex = Application.isMobilePlatform ? _mobileIndex : _pcIndex;
        /*
        _collectionScreen.SetContent(_contentsCollections[Application.isMobilePlatform ? _mobileIndex : _pcIndex]);
        */

        _collectionScreen.Init(
            _collectionMovements[_currentIndex], _allCollectionItems[_currentIndex],
            _contentsCollections[_currentIndex]);

        /*
        _collectionScreen.SetListCollections(Application.isMobilePlatform
            ? _allCollectionItemsMobile
            : _allCollectionItemsPC);*/
    }

    private void Start()
    {
    }
}