using System.Collections.Generic;
using ItemContent;
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
        
        public bool IsMapWithoutProfit => _isMapWithoutProfit;
        
        public bool IsMapExpanding => _isMapExpanding;

        public Transform ItemsContainer => _itemsContainer;
        
        public Transform RoadsContainer => _roadsContainer;

        public int Index => _index;
        
        public List<Item> StartItems => _startItems;
    }
}
