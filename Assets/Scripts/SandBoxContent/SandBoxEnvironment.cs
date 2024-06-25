using ItemPositionContent;
using UnityEngine;

namespace SandBoxContent
{
    public class SandBoxEnvironment : MonoBehaviour
    {
        [SerializeField] private ItemPosition[] _itemPositions;
        [SerializeField] private SandBoxStorage _sandBoxStorage;

        private void Start()
        {
            foreach (ItemPosition itemPosition in _itemPositions)
                itemPosition.GetComponent<FinderPositions>().FindNeighbor();

            _sandBoxStorage.LoadDataInfo();
        }
    }
}