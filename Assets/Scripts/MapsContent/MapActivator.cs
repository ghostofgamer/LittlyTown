using InitializationContent;
using UnityEngine;

namespace MapsContent
{
    public class MapActivator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _maps;
        [SerializeField] private Initializator _initializator;

        public void ChangeActivityMaps()
        {
            for (int i = 0; i < _maps.Length; i++)
            {
                if (i != _initializator.Index)
                    _maps[i].SetActive(false);
            }
        }

        public void ActivateAllMaps()
        {
            foreach (GameObject map in _maps)
                map.SetActive(true);
        }
    }
}