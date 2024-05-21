using UnityEngine;

namespace MapsContent
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private int _index;

        public int Index => _index;
    }
}
