using System;
using System.Collections;
using CountersContent;
using ItemContent;
using ItemPositionContent;
using UI.Buttons.PossibilitiesFiles;
using UnityEngine;

namespace PossibilitiesContent
{
    public class RemovalItems : MonoBehaviour
    {
        [SerializeField] private DeleteButton _deleteButton;
        [SerializeField] private PossibilitiesCounter _positionsCounter;
        [SerializeField] private AudioSource _audioSource;

        private int _layerMask;
        private int _layer = 3;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.15f);
        private RaycastHit _hit;
        private Ray _ray;

        public event Action Removed;

        public event Action Removing;

        public event Action<Item> ItemRemoved;

        public bool IsWorking { get; private set; }

        private void OnEnable()
        {
            _deleteButton.RemovalActivated += OnActivateWork;
            _deleteButton.RemovalDeactivated += OnDeactivateWork;
        }

        private void OnDisable()
        {
            _deleteButton.RemovalActivated -= OnActivateWork;
            _deleteButton.RemovalDeactivated -= OnDeactivateWork;
        }

        private void Start()
        {
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
        }

        public void StartRemove()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Remove());
        }

        private IEnumerator Remove()
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                if (_hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy)
                {
                    ItemRemoved?.Invoke(itemPosition.Item);
                    itemPosition.Item.Deactivation();
                    itemPosition.Item.gameObject.SetActive(false);
                    itemPosition.ClearingPosition();
                    _audioSource.PlayOneShot(_audioSource.clip);
                    yield return null;
                    _positionsCounter.DecreaseCount();
                    Removing?.Invoke();
                    yield return _waitForSeconds;
                    Removed?.Invoke();
                }
            }
        }

        private void OnActivateWork()
        {
            IsWorking = true;
        }

        private void OnDeactivateWork()
        {
            IsWorking = false;
        }
    }
}