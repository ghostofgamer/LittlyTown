using System;
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
    [SerializeField] private Transform _DescriptionCollection;
    [SerializeField] private CameraMovement _cameraMovement;
    
    private List<Item>[] _allCollectionItems = new List<Item>[2];

    private int _pcIndex = 0;
    private int _mobileIndex = 1;
    private int _currentIndex;
    private List<Transform> _descriptions = new List<Transform>();

    public event Action<int> PlatformSelected;

    private void Awake()
    {
        // _currentIndex = Application.isMobilePlatform ? _pcIndex : _mobileIndex;
        _currentIndex = Application.isMobilePlatform ? _mobileIndex : _pcIndex;
        PlatformSelected?.Invoke(_currentIndex);
        _allCollectionItems[_pcIndex] = _allCollectionItemsPC;
        _allCollectionItems[_mobileIndex] = _allCollectionItemsMobile;
        _collectionScreen.Init(
            _collectionMovements[_currentIndex], _allCollectionItems[_currentIndex],
            _contentsCollections[_currentIndex]);

        /*if (_currentIndex == _mobileIndex)
        {
            ChangePositionDescriptions();
        }*/

        if (_currentIndex == _mobileIndex)
        {
            _cameraMovement.Init(45,9);
        }
        else
        {
            _cameraMovement.Init(30,6);
        }
    }

    private void ChangePositionDescriptions()
    {
        for (int i = 0; i < _DescriptionCollection.childCount; i++)
            _descriptions.Add(_DescriptionCollection.GetChild(i));


        foreach (var description in _descriptions)
        {
            Vector2 position = description.GetComponent<RectTransform>().anchoredPosition;
            position.y = 600f;
            description.GetComponent<RectTransform>().anchoredPosition = position;
        }
    }
}