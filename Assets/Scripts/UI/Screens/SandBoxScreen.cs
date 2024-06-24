using System.Collections;
using System.Collections.Generic;
using CameraContent;
using Enums;
using EnvironmentContent;
using ItemPositionContent;
using Newtonsoft.Json;
using SandBoxContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class SandBoxScreen : AbstractScreen
    {
        [SerializeField] private List<ItemPosition> _itemPositions;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
        [SerializeField] private CameraScrolling _cameraScrolling;
        [SerializeField] private TurnEnvironment _turnEnvironment;
        [SerializeField] private GameObject _environment;
        [SerializeField] private EnvironmentMovement _environmentMovement;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private List<ItemButton> _itemButtons = new List<ItemButton>();
        [SerializeField] private CollectionScreen _collectionScreen;
    
        private List<Items> _collectedItems = new List<Items>();
        private bool _isActive;
        private Coroutine _coroutine;
        private float _timeElapsed;
        private float _durationTime = 1f;
        private Vector3 _targetPosition;

        public override void Open()
        {
            base.Open();
            _visualItemsDeactivator.SetPositions(_itemPositions);
            _cameraScrolling.enabled = true;
            _environmentMovement.GoAway();
            // _turnEnvironment.SetEnvironment(_environment);
            ReturnPosition();
            // _cameraMovement.ZoomOut();
            _cameraMovement.ZoomIn();
            _scrollRect.enabled = (!(_scrollbar.size >= 1));
            _scrollbar.gameObject.SetActive(!(_scrollbar.size >= 1));
            StartCoroutine(ShowButtons());
        }

        public override void Close()
        {
            base.Close();
            _environmentMovement.ReturnPosition();
            GoAway();
            _cameraMovement.ResetZoom();
        }

        private void GoAway()
        {
            _targetPosition = new Vector3(_environment.transform.position.x, _environment.transform.position.y,
                _environment.transform.position.z + 500);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isActive = false;
            _coroutine = StartCoroutine(Move(_environment.transform.position, _targetPosition));
        }

        private void ReturnPosition()
        {
            _environment.transform.position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z - 500);
            _environment.SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isActive = true;
            _coroutine = StartCoroutine(Move(_environment.transform.position, _startPosition));
        }

        private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
        {
            _timeElapsed = 0f;

            while (_timeElapsed < _durationTime)
            {
                _timeElapsed += Time.deltaTime;
                _environment.transform.position = Vector3.Lerp(startPosition, targetPosition, _timeElapsed / _durationTime);
                yield return null;
            }

            _environment.transform.position = targetPosition;
            _environment.SetActive(_isActive);
        }

        private void DeserializeCollectedItemsFromJson(string json)
        {
            _collectedItems = JsonConvert.DeserializeObject<List<Items>>(json);
        }

        private void LoadCollectedItemsFromPlayerPrefs()
        {
            string json = PlayerPrefs.GetString("CollectedItems");

            if (!string.IsNullOrEmpty(json))
                DeserializeCollectedItemsFromJson(json);
        }
    
        private IEnumerator ShowButtons()
        {
            LoadCollectedItemsFromPlayerPrefs();
            yield return null;
        
            for (int i = 0; i < _itemButtons.Count; i++)
            {
                if (_collectionScreen.CollectedItems.Contains(_itemButtons[i].ItemName))
                    _itemButtons[i].UnblockButton();
                else
                    _itemButtons[i].BlockButton();
            }
        }
    }
}