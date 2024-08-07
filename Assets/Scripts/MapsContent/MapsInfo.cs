using UI;
using UnityEngine;

namespace MapsContent
{
    public class MapsInfo : MonoBehaviour
    {
        [SerializeField] private GameObject[] _mapInfoObjects;
        [SerializeField] private ChooseMap _chooseMap;
        [SerializeField] private Records _records;

        private void OnEnable()
        {
            _chooseMap.MapChanged += OnActivatedInfo;
        }

        private void OnDisable()
        {
            _chooseMap.MapChanged -= OnActivatedInfo;
        }

        public void OnActivatedInfo(int index)
        {
            foreach (var mapInfo in _mapInfoObjects)
                mapInfo.SetActive(false);

            _mapInfoObjects[index].SetActive(true);
            _records.OnShow();
        }
    }
}