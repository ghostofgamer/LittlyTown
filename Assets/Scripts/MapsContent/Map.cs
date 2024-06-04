using UnityEngine;

namespace MapsContent
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private Transform _roadsContainer;
        [SerializeField] private int _index;
        
        public Transform ItemsContainer => _itemsContainer;
        
        public Transform RoadsContainer => _roadsContainer;

        public int Index => _index;
    }
}
