using UnityEngine;

namespace MapsContent
{
    public class InputChooseMap : MonoBehaviour
    {
        [SerializeField] private ChooseMap _chooseMap;
        
        private void Update()
        {
            if (!_chooseMap.IsWork)
                return;
            
            if (Input.GetMouseButtonDown(0))
                _chooseMap.SetStartPosition();
            
            if (Input.GetMouseButton(0))
                _chooseMap.MoveMapTouching();
            
            if (Input.GetMouseButtonUp(0))
                _chooseMap.CheckImpulse();
        }
    }
}
