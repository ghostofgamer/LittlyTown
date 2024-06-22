using UnityEngine;

namespace PossibilitiesContent
{
    public class InputReplacementPosition : MonoBehaviour
    {
        [SerializeField] private ReplacementPosition _replacementPosition;
    
        private bool _isLooking;
    
        private void Update()
        {
            if (!_replacementPosition.IsWorking)
                return;

            if (Input.GetMouseButtonDown(0))
                _isLooking = true;
            
            if (Input.GetMouseButtonUp(0))
            {
                _isLooking = false;
                _replacementPosition.StartReplace();
            }

            if (_isLooking)
                _replacementPosition.ActivateVisualPosition();
        }
    }
}
