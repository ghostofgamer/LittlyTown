using System.Collections;
using UnityEngine;

namespace MapsContent
{
    public class InputChooseMap : MonoBehaviour
    {
        [SerializeField] private ChooseMap _chooseMap;

        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.3f);
        private bool _isWork;

        private void Start()
        {
            enabled = false;
        }

        private void Update()
        {
            if (!_isWork)
                return;

            if (Input.GetMouseButtonDown(0))
                _chooseMap.SetStartPosition();

            if (Input.GetMouseButton(0))
                _chooseMap.MoveMapTouching();

            if (Input.GetMouseButtonUp(0))
                _chooseMap.CheckDragDistance();
        }

        public void StartWork()
        {
            StartCoroutine(EnableWork());
        }

        public void StopWork()
        {
            _isWork = false;
            enabled = false;
        }

        private IEnumerator EnableWork()
        {
            yield return _waitForSeconds;
            _isWork = true;
        }
    }
}