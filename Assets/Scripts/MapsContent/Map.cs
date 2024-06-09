using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace MapsContent
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private Transform _roadsContainer;
        [SerializeField] private int _index;
        [SerializeField] private List<Item> _startItems = new List<Item>(); 
        [SerializeField]private bool _isMapWithoutProfit = false;
        [SerializeField]private bool _isMapExpanding = false;
        [SerializeField] private ItemPosition[] _randomPositionWaters;
        [SerializeField] private Territory[] _expandingTerritories;
        [SerializeField] private bool _isWaterRandom;
        [SerializeField] private Transform _mover;

        public Transform Mover => _mover;
        
        public Territory[] ExpandingTerritories => _expandingTerritories;

        public bool IsWaterRandom => _isWaterRandom;
        
        public ItemPosition[] RandomPositionWaters => _randomPositionWaters;
        
        public bool IsMapWithoutProfit => _isMapWithoutProfit;
        
        public bool IsMapExpanding => _isMapExpanding;

        public Transform ItemsContainer => _itemsContainer;
        
        public Transform RoadsContainer => _roadsContainer;

        public int Index => _index;
        
        public List<Item> StartItems => _startItems;
    }
}
