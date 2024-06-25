using UnityEngine;

namespace MapsContent
{
    public class Territory : MonoBehaviour
    {
        [SerializeField] private PositionScaller[] _positionScallers;
        [SerializeField] private bool _isExpanding;

        public bool IsExpanding => _isExpanding;

        public bool IsOpened { get; private set; }

        public void PositionActivation()
        {
            foreach (var positionScaller in _positionScallers)
            {
                positionScaller.gameObject.SetActive(true);
                positionScaller.ScaleChanged();
            }
        }

        public void ShowPositions()
        {
            foreach (var positionScaller in _positionScallers)
                positionScaller.gameObject.SetActive(true);
        }

        public void EnableOpened()
        {
            IsOpened = true;
        }

        public void DisableOpened()
        {
            IsOpened = false;
        }
    }
}